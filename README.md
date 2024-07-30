Unity Developer Task: Domino Tile Placement

Task Description

Your task is to create the logic for placing domino tiles based on a given array. Each sub-array
in the array represents a domino tile, with the order of the array determining the sequence in
which the tiles are placed. Additionally, the first tile played will be provided as a sub-array
representation, such as [6, 6].

Task Requirements

1. Array Representation: The array provided consists of sub-arrays, where each element
represents a domino tile to be placed. For example: [[4, 2], [2, 6], [6, 6],
[6, 5], [5, 5], [5, 3]]. The order of the array determines the sequence in
which the tiles are placed.

2. First Tile Information: Along with the array, you will receive a tile that is already placed
on the board, represented as an array such as [6, 6]. This information serves as the
starting point for placing subsequent tiles.

3. Tile Placement Constraints:
○ Maximum of 7 tiles in a single vertical column, with 3 tiles above and 3 tiles below
the first stone.
○ When 3 tiles are reached above or below the first stone, the next tile should be
placed horizontally to create a U-turn pattern.
○ Doubles (e.g., [5, 5]) should be placed horizontally.

4. Visual Representation: You can find a visual prototype here
[https://www.figma.com/proto/ec5NyHJqMYpr7uo4xpaNkw/Unity-Dev-Dominoes-Test?no
de-id=1-1572&t=xyZ6hdKxLLxQNEf5-1&scaling=scale-down&content-scaling=fixed](https://www.figma.com/proto/ec5NyHJqMYpr7uo4xpaNkw/Unity-Dev-Dominoes-Test?node-id=1-1572&t=xyZ6hdKxLLxQNEf5-1&scaling=scale-down&content-scaling=fixed)

5. Subscription to Events: Your script should subscribe to an event in C# code, simulating
an event received from the server. This event will occur every couple of seconds and will
provide updated information, including the tiles array and the initial tile.

Additional Information
● Sprites: You will be provided with 28 tile sprites and additional sprites to create a
background for visualization.
● C# Code: The event to which your tile placement script should subscribe will provide
updated information, mimicking real-time updates as if a player has placed a tile.
○ Static Event Name: GameState.OnDominoesTilesChainUpdate
● Background Color: #516970
● Scene Name: Jawaker
