package graph

import (
	"fmt"
	"sync"
)

type Graph struct {
	NodeNumber      int
	Graph           [][]int
	Update          [][]int
	InitData        [][]int
	Neighbour       [][]int
	IsUpdatePresent []bool
}

func (G *Graph) Init() {
	fmt.Println("Enter the number of nodes in the graph")
	fmt.Scanln(&G.NodeNumber)
	fmt.Println(G.NodeNumber)

	G.Graph = make([][]int, G.NodeNumber)
	G.InitData = make([][]int, G.NodeNumber)
	G.Update = make([][]int, G.NodeNumber)
	G.Neighbour = make([][]int, G.NodeNumber)
	G.IsUpdatePresent = make([]bool, G.NodeNumber)

	for i := 0; i < G.NodeNumber; i++ {
		G.Graph[i] = make([]int, G.NodeNumber)
		G.Update[i] = make([]int, G.NodeNumber)
		G.Neighbour[i] = make([]int, G.NodeNumber)
		G.InitData[i] = make([]int, G.NodeNumber)
	}
	for i := 0; i < G.NodeNumber; i++ {
		for j := 0; j < G.NodeNumber; j++ {
			G.Graph[i][j] = -1
			G.Update[i][j] = 999
		}
	}

	G.Display()
}

func (G *Graph) AddData() {
	fmt.Println("Enter the Distance between the nodes. 99 for infinity")
	for i := 0; i < G.NodeNumber; i++ {
		for j := 0; j < G.NodeNumber; j++ {
			if i == j {
				G.Graph[i][j] = 0
				G.Graph[j][i] = 0
			}
			if G.Graph[i][j] == -1 {
				fmt.Println("Node", i, "and", j, ": ")
				fmt.Scanln(&G.Graph[i][j])
				G.Graph[j][i] = G.Graph[i][j]
				if G.Graph[i][j] != 999 {
					G.Neighbour[i][j] = 1
					G.Neighbour[j][i] = G.Neighbour[i][j]
				}
			}
			G.InitData[i][j] = G.Graph[i][j]
		}
	}

	G.Display()
}

func (G *Graph) Display() {
	for i := 0; i < G.NodeNumber; i++ {
		fmt.Println(G.Graph[i])
	}
}

func (G *Graph) UpdateData(Node int) {
	for i := 0; i < G.NodeNumber; i++ {
		if i != Node && G.Graph[Node][i] > G.Update[Node][i] {
			G.Graph[Node][i] = G.Update[Node][i]
		}
	}
	G.IsUpdatePresent[Node] = false
}

func (G *Graph) SendData(wg *sync.WaitGroup, Sender int, Node int) {
	for i := 0; i < G.NodeNumber; i++ {
		if i != Node {
			v := G.Graph[Sender][i] + G.Graph[Sender][Node]
			if v < G.Update[Node][i] {
				G.Update[Node][i] = v
				G.IsUpdatePresent[Node] = true
			}
		}
	}
	wg.Done()
}

func (G *Graph) Run() {
	var k int
	var wg sync.WaitGroup
	for j := 0; j < G.NodeNumber; j++ {
		if G.Neighbour[0][j] == 1 {
			wg.Add(1)
			go G.SendData(&wg, 0, j)

		}
	}
	wg.Wait()
	for i := 0; i < G.NodeNumber; i = (i + 1) % G.NodeNumber {
		if G.IsUpdatePresent[i] {
			G.UpdateData(i)

			for j := 0; j < G.NodeNumber; j++ {
				if G.Neighbour[i][j] == 1 {
					wg.Add(1)
					G.SendData(&wg, i, j)

				}
			}
			wg.Wait()
		}

		for k = 0; k < G.NodeNumber && G.IsUpdatePresent[k] == false; k++ {
		}
		if k == G.NodeNumber && i == G.NodeNumber-1 {
			break
		}
	}
	G.Display()

}

func (G *Graph) ChangeData() {
	var i, j, c int
	fmt.Println("Enter the nodes and new cost")
	fmt.Scanln(&i, &j, &c)
	G.Graph = G.InitData

	G.Graph[i][j] = c
	G.Graph[j][i] = c
	for ii := 0; ii < G.NodeNumber; ii++ {
		for jj := 0; jj < G.NodeNumber; jj++ {
			G.Update[ii][jj] = 999
		}
	}
}
