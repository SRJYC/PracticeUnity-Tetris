# Practise-Tetris
A unity project for practise. This project replicate Tetris.

## Preview
![](Preview.gif)

## Online Reference

I found existing code online. And just use part of it, such as the playfield, how to delete and move rows. 
But I don't like the rest of code, so I do it on my way.

## Group, Block, and Scriptable Object

The different types of group are scriptable objects.

Each of them contain 4 positions. Each position represent a block. 
The position is the index of the block in playfield since playfield is just a 2D array of blocks.

Therefore, playfield just change color to indicate if the block is filled or not instead of create new game objects and change parent.

## Command Action and Scriptable Object

The Next Action class is also an scriptable object. It contains the next action for controller to perform.

The four buttons just change the Next Action object.

When it's time to perform next action, controller will look into the Next Action object and get the instruction.
