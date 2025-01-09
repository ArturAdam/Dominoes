# How It Works

## Mode Selection: 
Upon launch, you decide between random tile selection (`R`) or manual tile editing (`M`).
   
## Tile Selection:
   - In **Random Mode**, the application automatically generates 6 random dominoes.
   - In **Manual Mode**, you interactively create dominoes using the keyboard. Instructions and current tile status are displayed, guiding you through the selection process.
     
  ### Manual Mode Instructions:
   - [<-] and [->] to navigate between domino's sides.
   - [+] or up arrow key to increase the current side value.
   - [-] or down arrow key to decrease the current side value.
   - [A] to add the current tile to selected tiles.
   - [R] to remove the most recently added tile.
   - [F] to finish and check for a circular chain.
     
 ## Chain Validation:
   - After tile selection, the program creates a graph of the domino tiles and attempts to form a circular chain.
   - If a valid circular arrangement is found, it's displayed in the console.
   - If not, an error message explains that a circular chain cannot be formed with the selected tiles.
