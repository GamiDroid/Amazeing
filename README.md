# Amazeing

1. Enter maze when the player is not already in a maze
      1. When starting the application and player is already in a maze. forget player state
      2. register player
 
Dit ga ik handmatig doen.

 2. When entering a maze. The possible actions
      1. When current bag score equals total available score.
          1. Go to exit point.
          2. Check whether the exit point is already found.
          3. if found search route to exit point. (BFS)
          4. Go to tiles that are not visited.
      2. When current player score equals to total available score.
          1. Go to collection point
          2. Check whether the collection point is already found.
          3. If found search route to collection point. (BFS)
          4. Go to tiles that are not visited.
      2. When amount of known tiles equals to total maze tiles
          1. Not all scores are found. Go to the tiles (BFS)
      3. Move to the next tile.
          1. Go to the next file bases on the highest score.