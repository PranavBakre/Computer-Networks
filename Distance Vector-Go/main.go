//Execution Instructions:

/*
	Building the application: go build main.go
	Directly executing applcation : go run main.go
	Executing the pre built binary: ./main
	Location of the package graph: Graph Folder n the project

*/

/*
	Enter the Nodes and the New Cost:
		i j cost;
		i,j= Node number


*/

package main

import (
	graph "./Graph"
)

func main() {
	var G graph.Graph
	G.Init()
	G.AddData()
	G.Run()
	G.ChangeData()
	G.Run()
}
