# TruPlay-Assessment
Technical Assessment for TruPlay

Requirements Source: https://www.thesprucecrafts.com/war-card-game-rules-411145

## Implementation Notes:

All the art present in the game was either free on the Unity Asset Store or a royalty-free music source. Some of it has been edited by me to better fit this prototype, or to simplify it.

The specification didn't make mention specifically about starting a "War" with not enough cards, so I found this YouTube video that indicated the player that runs out of cards loses that war: https://youtu.be/z2K6uMX_g-U

I ran into an issue, where occassionally the cards would be shuffled in such a way there would be a never ending (or nearly never ending) game, because the card values synchronized and never tipped one way or the other. To counteract this, I made a slight change. Instead of the cards won being added immediately to the player's draw pile, when a Player runs out of cards the cards they have won are shuffled and then make their new draw deck. This ensures some variety is introduced to drastically reduce the potential for game deadlock.

Given that War is a game where there is not an element of strategy (unless playing with the alternate rules which I didn't end up getting implemented), there's no need to wait for player input to proceed with the game. My initial implementation had no graphical elements and just ran the game behind the scenes with logs to the console. Once that was working, I proceeded to add in some visuals and simple animations (forgive me, I'm a programmer!) to give some life to the game. I kept the automatic play progression in place, as forcing the user to press a button to play each card seemed unnecessary for this particular implementation.

Some next steps and/or things I would improve upon given more time include:
- Adding in the alternate play rules where the player can pick from 3 cards instead of always drawing the top card
- Improving animations
- Adding more sound effects where appropriate, such as background music
- Improve the game and menu scenes to be better visually
