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
