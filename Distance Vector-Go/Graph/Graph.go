package graph

import (
	"fmt"
	"sync"
	//	"text/scanner"
)

type Node struct { //Contents of every node
	//Current Data
	Distance  []int
	Next      []int
	Neighbour []int

	//Update Data
	UpdateDistance  []int
	UpdateNext      []int
	IsUpdatePresent bool
	//Initial Data
	InitDistance  []int
	InitNext      []int
	InitNeighbour []int
}

type Graph struct { //Network Map
	NodeNumber int
	Graph      []Node
}

func (G *Graph) Init() { //Initialises the network map
	fmt.Println("Enter the number of nodes in the graph")
	fmt.Scanln(&G.NodeNumber)
	fmt.Println(G.NodeNumber)

	G.Graph = make([]Node, G.NodeNumber)
	for i := 0; i < G.NodeNumber; i++ {
		//Node Current Data Allocation
		G.Graph[i].Distance = make([]int, G.NodeNumber)
		G.Graph[i].Neighbour = make([]int, G.NodeNumber)
		G.Graph[i].Next = make([]int, G.NodeNumber)
		//Update Data Allocation
		G.Graph[i].UpdateDistance = make([]int, G.NodeNumber)
		G.Graph[i].UpdateNext = make([]int, G.NodeNumber)

		//Init Data allocation
		G.Graph[i].InitDistance = make([]int, G.NodeNumber)
		G.Graph[i].InitNeighbour = make([]int, G.NodeNumber)
		G.Graph[i].InitNext = make([]int, G.NodeNumber)

	}
	for i := 0; i < G.NodeNumber; i++ {
		for j := 0; j < G.NodeNumber; j++ {
			G.Graph[i].Distance[j] = -1
			G.Graph[i].Next[j] = -1
			G.Graph[i].Neighbour[j] = 0
			G.Graph[i].UpdateDistance[j] = 999
			G.Graph[i].UpdateNext[j] = -1
		}
	}

	G.Display()
}

func (G *Graph) AddData() { //Adding the data into the map
	fmt.Println("Enter the Distance between the nodes. 999 for infinity")
	for i := 0; i < G.NodeNumber; i++ {

		for j := 0; j < G.NodeNumber; j++ {

			if i == j {
				G.Graph[i].Distance[j] = 0
				G.Graph[j].Distance[i] = 0
			}

			if G.Graph[i].Distance[j] == -1 {
				fmt.Println("Node", i, "and", j, ": ")
				fmt.Scanln(&G.Graph[i].Distance[j])
				G.Graph[j].Distance[i] = G.Graph[i].Distance[j]
				if G.Graph[i].Distance[j] != 999 {
					G.Graph[i].Neighbour[j] = 1 //G.Neighbour[i][j] = 1
					G.Graph[j].Neighbour[i] = 1 //G.Neighbour[j][i] = G.Neighbour[i][j]
					G.Graph[i].Next[j] = j
					G.Graph[j].Next[i] = i
				}
			}
			G.Graph[i].InitDistance[j] = G.Graph[i].Distance[j]
			G.Graph[i].InitNext[j] = G.Graph[i].Next[j]
			G.Graph[i].InitNeighbour[j] = G.Graph[i].Neighbour[j]
		}
	}
	fmt.Println("Initial Data")
	G.Display()
}

func (G *Graph) Display() { //Display the node data
	fmt.Println("Node No. Distance Vector\tNext Hop\t Neighbour")
	for i := 0; i < G.NodeNumber; i++ {

		fmt.Printf("%d \t%+v\t\t%+v\t%+v\n", i, G.Graph[i].Distance, G.Graph[i].Next, G.Graph[i].Next)
	}
	fmt.Println("*************************************************\n1.Neighbour indicated by 1.Node Number=Position\nNext Hop:=-1 implies no next hop.i.e. Node itself\n*************************************************")
}

func (G *Graph) UpdateData(Node int) { //Function to update the distance vector at every node
	for i := 0; i < G.NodeNumber; i++ {
		if i != Node && G.Graph[Node].Distance[i] > G.Graph[Node].UpdateDistance[i] { //if i != Node && G.Graph[Node].Distance[i] > G.Update[Node].Distance[i] {
			G.Graph[Node].Distance[i] = G.Graph[Node].UpdateDistance[i] //G.Update[Node].Distance[i]
			G.Graph[Node].Next[i] = G.Graph[Node].UpdateNext[i]         //G.Update[Node].Next[i]
		}
	}
	//G.IsUpdatePresent[Node] = false
	G.Graph[Node].IsUpdatePresent = false
}

func (G *Graph) SendData(wg *sync.WaitGroup, Sender int, Node int) { //Concurrently send the data to all neighbours
	for i := 0; i < G.NodeNumber; i++ {
		if i != Node {
			v := G.Graph[Sender].Distance[i] + G.Graph[Sender].Distance[Node]
			if v < G.Graph[Node].UpdateDistance[i] { //G.Update[Node].Distance[i] {
				//G.Update[Node].Distance[i] = v
				G.Graph[Node].UpdateDistance[i] = v
				//G.Update[Node].Next[i] = Sender
				G.Graph[Node].UpdateNext[i] = Sender
				//G.IsUpdatePresent[Node] = true
				G.Graph[Node].IsUpdatePresent = true
			}
		}
	}
	wg.Done()
}

func (G *Graph) Run() { //Emulate the running of distance vector
	var k int
	var wg sync.WaitGroup
	for j := 0; j < G.NodeNumber; j++ {
		if G.Graph[0].Neighbour[j] == 1 { //if G.Neighbour[0][j] == 1 {
			wg.Add(1)
			go G.SendData(&wg, 0, j)

		}
	}
	wg.Wait()
	for i := 0; i < G.NodeNumber; i = (i + 1) % G.NodeNumber {
		if G.Graph[i].IsUpdatePresent { //G.IsUpdatePresent[i] {
			G.UpdateData(i)

			for j := 0; j < G.NodeNumber; j++ {
				if G.Graph[i].Neighbour[j] == 1 { //if G.Neighbour[i][j] == 1 {
					wg.Add(1)
					G.SendData(&wg, i, j)

				}
			}
			wg.Wait()
		}

		for k = 0; k < G.NodeNumber && G.Graph[k].IsUpdatePresent == false; k++ {
		}
		if k == G.NodeNumber && i == G.NodeNumber-1 {
			break
		}
	}
	fmt.Println("Final Data")
	G.Display()

}

func (G *Graph) ChangeData() { //Change the data of the nodes to indicate broken connection
	var i, j, c int

	var choice = "y"
	for i := 0; i < G.NodeNumber; i++ {
		G.Graph[i].Distance = G.Graph[i].InitDistance
		G.Graph[i].Next = G.Graph[i].InitNext
		G.Graph[i].Neighbour = G.Graph[i].InitNeighbour
	}
	for choice == "y" {
		fmt.Println("Enter the nodes and new cost")
		fmt.Scanln(&i, &j, &c)
		if i != j {
			G.Graph[i].Distance[j] = c
			G.Graph[j].Distance[i] = c
			if c != 999 {
				G.Graph[i].Next[j] = j
				G.Graph[j].Next[i] = i
			}
		}
		fmt.Println("Enter another value? y/n")
		fmt.Scanln(&choice)
	}
	for ii := 0; ii < G.NodeNumber; ii++ {
		for jj := 0; jj < G.NodeNumber; jj++ {
			G.Graph[ii].UpdateDistance[jj] = 999 //G.Update[ii].Distance[jj] = 999
		}
	}
}
