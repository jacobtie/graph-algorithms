# Assignment II - Graph Algorithms

## How to Run

To run the program, make sure you have .NET Core 3.0 installed. You can run `dotnet run` to run the program. This will implicitly run `dotnet restore` and `dotnet build` before running `dotnet run`. Keep in mind that this will create `bin/` and `obj/` directories. 
  
The easiest way to run/edit/debug this application is by using Visual Studio Code. When you open the project is VSCode, it will prompt you to add required assets to the project. This will generate a `.vscode` folder with `launch.json` and `tasks.json` files that allow one to debug the program. 

To input graph files, please add a `.txt` file to the `input-files` directory in the root. The program will prompt you for the name of your file. If, for instance, you add `mygraph.txt` to the `input-files` directory, you would enter `mygraph.txt` in the console when prompted. 

Please ensure that your input files uses a single white-space character (such as a space or tab) as a delimiter between the columns of the file. The format is the same as given by the assignment prompt. Please also ensure that your input graph does not contain double edges, as this interferes with printing the adjacency matrix. Furthermore, if using a graph for Dijkstra's algorithm, please ensure that there are no negative edges and that a source node is included at the bottom. 

## Making Changes

We will loosely be following GitFlow in making changes to this repository. Please remember to make a new branch off of the dev branch for the sorts you add and make a pull request to be reviewed when finished. We will make the merge to master when all of the sorts are finished. 
