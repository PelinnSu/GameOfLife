📌 Overview

The Game of Life starts with a simple grid of cells. Each cell is either alive or dead, and at first, nothing seems special. But once the simulation begins, the grid comes to life.
At every step, each cell looks at its neighbors and quietly decides its fate. Some survive, some disappear, and new ones are born. No cell knows the bigger picture — it only follows a few basic rules. And yet, over time, patterns emerge. Shapes repeat, structures move across the grid, and complex behavior appears where there was none before.
There is no goal, no winning condition, and no player control after the start. The only input is the initial pattern. Everything that happens afterward is the result of simple rules interacting over time — a small world evolving on its own.

## ⚙️ Rules of the Game

Each cell interacts with its 8 neighboring cells and follows these rules:

- **Alive cell**
  - Fewer than 2 live neighbors → dies (underpopulation)
  - 2 or 3 live neighbors → survives
  - More than 3 live neighbors → dies (overpopulation)

- **Dead cell**
  - Exactly 3 live neighbors → becomes alive (reproduction)

