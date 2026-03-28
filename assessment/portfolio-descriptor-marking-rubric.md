# Portfolio

<img src="../resources (ignore)/img/logo.jpg" alt="Otago Polytechnic Logo" width="200" height="auto" />

# ID623002: Introductory Game Development

## Assessment Information

| Level | Credits | Assessment Type  | Weighting |
| ----- | ------- | ---------------- | --------- |
| 6     | 15      | Individual       | 100%      |

## Assessment Overview

In this **individual** assessment, you will develop **three** games. In addition, marks will be allocated for code quality and best practices, documentation and **Git** usage.

## Learning Outcome

At the successful completion of this course, learners will be able to:

1. Design and build usable, attractive games using various introductory algorithms following an appropriate software development methodology.

## Assessments

| Assessment | Weighting | Due Date           | Learning Outcome |
| ---------- | --------- | ------------------ | ---------------- |
| Portfolio  | 100%      | 26 June at 4.59 PM | 1                |

## Conditions of Assessment

You will complete this assessment during your learner-managed time. However, there will be time during class to discuss the requirements and your progress on this assessment. This assessment will need to be completed by **26th June** at **4.59 PM**.

## Pass Criteria

This assessment is criterion-referenced (CRA) with a cumulative pass mark of **50%** over all assessments in **ID623002: Introductory Game Development**.

## Submission

You **must** submit all application files via **GitHub Classroom**.

- Repository URL: [https://classroom.github.com/a/tSkpt5Ho](https://classroom.github.com/a/tSkpt5Ho)
- Late Penalty: 10% per day, rolling over at 5.00 PM

## Authenticity

All parts of your submitted assessment **must** be completely your work. Do your best to complete this assessment without using an **AI generative tool**. You need to demonstrate to the course lecturer that you can meet the learning outcome for this assessment.

### AI Tools

Learning to use AI tools is an important skill. While AI tools are powerful, you **must** be aware of the following:

- If you provide an AI tool with a prompt that is not refined enough, it may generate a not-so-useful response
- Do not trust the AI tool's responses blindly. You **must** still use your judgement and may need to do additional research to determine if the response is correct
- Acknowledge what AI tool you have used. In the assessment's repository **README.md** file, please include what prompt(s) you provided to the AI tool and how you used the response(s) to help you with your work

This also applies to code snippets retrieved from **StackOverflow** and **GitHub**.

Failure to do this may result in a mark of **zero** for this assessment.

## Policy on Submissions, Extensions, Resubmissions and Resits

The school's process concerning submissions, extensions, resubmissions and resits complies with **Otago Polytechnic** policies. Learners can view policies on the **Otago Polytechnic** website located at [https://www.op.ac.nz/about-us/governance-and-management/policies](https://www.op.ac.nz/about-us/governance-and-management/policies).

### Extensions

Familiarise yourself with the assessment due date. Extensions will **only** be granted if you are unable to complete the assessment by the due date because of **unforeseen circumstances outside your control**. The length of the extension granted will depend on the circumstances and **must** be negotiated with the course lecturer before the assessment due date. A medical certificate or support letter may be needed. Extensions will not be granted for poor time management or pressure of other assessments.

### Resits

Resits and reassessments **are not** applicable in **ID623002: Introductory Game Development**.

---

## Additional Information

- **Do not** rewrite your **Git** history. It is important that the course lecturer can see how you worked on your assessment over time.
- You need to show the course lecturer the initial **GitHub** issues before you start your development work. Following this, you need to show the course lecturer your **GitHub** issues at the end of each week.

---

## Functionality - Learning Outcome 1 (50%)

---

### Breakout - 15% 

- Build and publish the game to **itch.io**.

#### Fundamental Mechanics

- **Main menu** - The game should have a main menu with options to start the game, view the high scores and exit the game.
- **Paddle movement** - The player should be able to move the paddle left and right using the **A** and **D** keys using the **Unity** input system.
- **Ball movement.**
- **Collisions** - The ball should bounce off the walls, paddle and bricks.
- **Brick formation** - The game should have a 10 x 4 grid of bricks.
- **Brick health** - Each brick should have a health value that determines how many hits it can take before being destroyed.
- **Score system** - One point should be awarded to the player who destroys a brick.
- **Score display** - The score of the player should be displayed on the screen.
- **Live system** - The player should have **3** lives.
- **Lives display** - The lives of the player should be displayed on the screen.
- **High scores** - The game should store and display the top **10** high scores.
- **Sound effects** - The game should have sound effects for the ball bouncing off the walls, paddle and bricks, scoring a point and the game ending.
- **Pause and resume** - The player should be able to pause and resume the game using the **P** key.
- **Game over** - The game should end when the player destroys all the bricks or loses all their lives.
- **End game menu** - The game should have an end game menu with options to play again, view the high scores and exit the game.

#### Tier 2 Mechanics

Select **three** mechanics from the list below:

- **Difficulty levels** - Easy, medium and hard difficulty levels that adjust the speed of the ball and health of the bricks.
- **Ball speed** - The speed of the ball should increase after a certain number of goals.
- **Countdown timer** - A countdown timer that starts the game after a certain number of seconds.
- **Customisation** - Asset options for the paddle and ball.
- **Music** - Background music that plays during the game.
- **Moving bricks** - Bricks that move horizontally.
- **Obstacles** - Obstacles that block the ball's path.

#### Tier 3 Mechanics

Select **two** mechanics from the list below:

- **Time limit** - A time limit to complete the game.
- **Three levels** - Three levels with different brick formations.
- **Sound settings** - Options to adjust the volume of the sound effects and music.
- **Brick repair** - Bricks that regenerate health over time.
- **Animated effects** - Animated destruction effects for the bricks.
- **Power-ups** - Random drops that allow the player to shoot multiple balls, increase the size of the paddle and slow down the ball.

#### Your Own Mechanics

Implement **three** mechanics of your choice. These mechanics should be different from the ones listed above.

---

### RogueLike - 20%

- Build and publish the game to **itch.io**.

#### Fundamental Mechanics

- **Main menu** - The game should have a main menu with options to start the game, view the high scores and exit the game.
- **Player movement** - The player should be able to move using the **Unity** input system.
- **Enemy movement** - The enemies should move towards the player when they are in range.
- **Combat** - The player and enemies should be able to attack each other.
- **Health system** - The player and enemies should have health values that decrease when attacked.
- **Health display** - The health of the player and enemies should be displayed on the screen.
- **Score system** - One point should be awarded to the player who defeats an enemy.
- **Score display** - The score of the player should be displayed on the screen.
- **Pickup system** - The player should be able to pick up items that increase their health and damage.
- **High scores** - The game should store and display the top **10** high scores.
- **Sound effects** - The game should have sound effects for the player attacking, the enemies attacking, the player being attacked and the game ending.
- **Pause and resume** - The player should be able to pause and resume the game using the **P** key.
- **Game over** - The game should end when the player defeats all the enemies or the player's health reaches zero.
- **End game menu** - The game should have an end game menu with options to play again, view the high scores and exit the game.

#### Tier 2 Mechanics

Select **three** mechanics from the list below:

- **Difficulty levels** - Easy, medium and hard difficulty levels that adjust the health and damage of the enemies.
- **Item variety** - Different items that provide unique effects to the player.
- **Customisation** - Asset options for the player, enemies and items.
- **Music** - Background music that plays during the game.
- **Random generation** - Randomly generated levels that change each playthrough.
- **Events** - Random events that occur during the game.
- **Multiple levels** - Multiple levels with different enemy types and layouts.

#### Tier 3 Mechanics

Select **two** mechanics from the list below:

- **Boss battles** - Boss enemies that have unique attack patterns and abilities.
- **Animated effects** - Animated effects for the player attacking, the enemies attacking and the game ending.
- **Sound settings** - Options to adjust the volume of the sound effects and music.
- **Fog of war** - A fog of war effect that hides parts of the map until the player explores them.
- **Minimap** - A minimap that shows the layout of the level and the player's position.
- **Teleportation** - Teleportation pads that allow the player to move between different parts of the level.

#### Your Own Mechanics

Implement **four** mechanics of your choice. These mechanics should be different from the ones listed above.

---

### Platformer - 15% 

- Build and publish the game to **itch.io**.

#### Fundamental Mechanics

- **Main menu** - The game should have a main menu with options to start the game, view the high scores and exit the game.
- **Player movement** - The player should be able to move left and right and jump using the **A**, **D** and **Space** keys using the **Unity** input system.
- **Gravity** - The player should fall when not on a platform.
- **Collisions** - The player should collide with platforms, walls, enemies and collectibles.
- **Platforms** - The game should have a variety of platforms for the player to navigate.
- **Enemy movement** - Enemies should patrol platforms and damage the player on contact.
- **Score system** - Points should be awarded to the player for collecting items and defeating enemies.
- **Score display** - The score of the player should be displayed on the screen.
- **Live system** - The player should have **3** lives.
- **Lives display** - The lives of the player should be displayed on the screen.
- **High scores** - The game should store and display the top **10** high scores.
- **Sound effects** - The game should have sound effects for jumping, landing, collecting items, defeating enemies and the game ending.
- **Pause and resume** - The player should be able to pause and resume the game using the **P** key.
- **Game over** - The game should end when the player completes all levels or loses all their lives.
- **End game menu** - The game should have an end game menu with options to play again, view the high scores and exit the game.

#### Tier 2 Mechanics

Select **three** mechanics from the list below:

- **Difficulty levels** - Easy, medium and hard difficulty levels that adjust the speed of the enemies and the number of hazards.
- **Collectibles** - Items scattered across levels that the player can collect for bonus points.
- **Moving platforms** - Platforms that move horizontally or vertically.
- **Countdown timer** - A countdown timer that adds urgency to completing each level.
- **Customisation** - Asset options for the player character and environment.
- **Music** - Background music that plays during the game.
- **Parallax scrolling** - A parallax scrolling effect for the background.
- **Checkpoint system** - Checkpoints that save the player's progress within a level.

#### Tier 3 Mechanics

Select **two** mechanics from the list below:

- **Multiple levels** - Three or more levels with increasing difficulty and different layouts.
- **Boss battles** - A boss enemy at the end of a level with unique attack patterns.
- **Power-ups** - Random drops that temporarily grant the player abilities such as increased speed, invincibility or double jump.
- **Animated effects** - Animated effects for jumping, landing, collecting items and defeating enemies.
- **Sound settings** - Options to adjust the volume of the sound effects and music.
- **Achievement system** - Achievements that reward the player for completing specific tasks.

#### Your Own Mechanics

Implement **three** mechanics of your choice. These mechanics should be different from the ones listed above.

---

## Code Quality and Best Practices - Learning Outcome 1 (40%)

---

### Project Structure

The codebase should be well-organised with a clear and logical structure that separates concerns and promotes reusability and maintainability.

---

### Naming Conventions

File, class, method, variable, constant and property names should be clear, descriptive and consistent across the codebase.

---

### Documentation and Comments

The codebase should be well-documented using the **XML** documentation format with comments that explain complex logic and clarify the purpose of classes, methods or complex sections.

---

### Code Formatting

Consistent indentation and spacing should be used across the codebase. This includes ensuring proper indentation for code blocks, following a standard format for braces and adding blank lines between sections.

---

### Dead Code

The codebase should not contain unused or redundant files, classes, methods, variables, constants or properties. Keeping unnecessary code around can lead to confusion, clutter and potential bugs down the line.

---

### Performance and Scalability

The codebase should be optimised for performance, using efficient algorithms and data structures to minimise bottlenecks.

---

## Documentation and Git Usage - Learning Outcome 1 (5%)

- Use **GitHub** issues to help you organise and prioritise your development work. The course lecturer needs to see consistent use of **GitHub** issues for the duration of the assessment.
- In your repository **README.md** file, proivide:
    - URLs to the games on **itch.io**.
    - A list of the mechanics you implemented for each game.
- A **.gitignore** file containing the ignored files in this resource - [https://raw.githubusercontent.com/github/gitignore/main/Unity.gitignore](https://raw.githubusercontent.com/github/gitignore/main/Unity.gitignore).
- Your **Git commit messages** should:
  - Reflect the context of each functional requirement change.
  - Be formatted using an appropriate naming convention style.

---

# Marking Rubric (100 marks)

---

## Functionality - Learning Outcome 1 (50 marks)

---

### Breakout (15 marks)

#### Fundamental Mechanics (7 marks)

| Band | Marks   | Criteria                                                                                                                                                                                                                                                                                                                                                                                                      |
| ---- | ------- | ------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| A    | 5.6-7   | All fundamental mechanics fully implemented and functional. Main menu, paddle movement (A/D keys via Unity input system), ball movement, collisions, 10x4 brick grid, brick health, score system with display, 3-life system with display, top 10 high scores, sound effects for all required events, pause/resume (P key) and end game menu all work correctly and cohesively. Game is published to itch.io. |
| B    | 4.5-5.5 | Most fundamental mechanics implemented and functional with minor issues. Majority of mechanics work correctly. Game is published to itch.io with most features accessible.                                                                                                                                                                                                                                    |
| C    | 3.5-4.4 | Core mechanics present but some are incomplete or buggy. Paddle, ball, collisions and basic score/lives system work, but some features such as high scores, sound effects or menus may be missing or partially functional. Game may be published to itch.io.                                                                                                                                                  |
| D/E  | 0-3.4   | Many fundamental mechanics missing or non-functional. Game is unplayable or fails to demonstrate the required features. May not be published to itch.io.                                                                                                                                                                                                                                                      |

#### Tier 2 Mechanics (3 marks)

| Band | Marks   | Criteria                                                                                                                                          |
| ---- | ------- | ------------------------------------------------------------------------------------------------------------------------------------------------- |
| A    | 2.4-3   | All three selected Tier 2 mechanics fully implemented and working seamlessly within the game. Mechanics enhance gameplay and are well-integrated. |
| B    | 2-2.3   | Three Tier 2 mechanics implemented with minor issues. Mechanics are functional and mostly integrated well with the rest of the game.              |
| C    | 1.5-1.9 | Fewer than three Tier 2 mechanics implemented, or three present but with notable bugs or poor integration.                                        |
| D/E  | 0-1.4   | Fewer than two Tier 2 mechanics implemented, or mechanics are largely non-functional.                                                             |

#### Tier 3 Mechanics (3 marks)

| Band | Marks   | Criteria                                                                                                                                     |
| ---- | ------- | -------------------------------------------------------------------------------------------------------------------------------------------- |
| A    | 2.4-3   | Both selected Tier 3 mechanics fully implemented and working seamlessly. Mechanics add meaningful depth to the game and are well-integrated. |
| B    | 2-2.3   | Both Tier 3 mechanics implemented with minor issues. Mechanics are functional and reasonably integrated.                                     |
| C    | 1.5-1.9 | Only one Tier 3 mechanic fully implemented, or both present but with significant issues or poor integration.                                 |
| D/E  | 0-1.4   | Tier 3 mechanics largely missing or non-functional.                                                                                          |

#### Your Own Mechanics (2 marks)

| Band | Marks   | Criteria                                                                                                                                                       |
| ---- | ------- | -------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| A    | 1.6-2   | Three original mechanics implemented that are clearly distinct from all listed mechanics. Each mechanic adds meaningful gameplay value and is well-integrated. |
| B    | 1.3-1.5 | Three original mechanics present with minor issues, or two fully implemented. Mechanics are generally distinct and add value.                                  |
| C    | 1-1.2   | Fewer than three original mechanics, or mechanics closely resemble listed ones. Some integration issues.                                                       |
| D/E  | 0-0.9   | Fewer than two original mechanics implemented, or mechanics are not meaningfully distinct or functional.                                                       |

---

### RogueLike (20 marks)

#### Fundamental Mechanics (10 marks)

| Band | Marks   | Criteria                                                                                                                                                                                                                                                                                                                                                                                                  |
| ---- | ------- | --------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| A    | 8-10    | All fundamental mechanics fully implemented and functional. Main menu, player movement (Unity input system), enemy movement (in-range tracking), combat, health system with display for player and enemies, score system with display, pickup system, top 10 high scores, sound effects for all required events, pause/resume (P key) and end game menu all work correctly. Game is published to itch.io. |
| B    | 6.5-7.9 | Most fundamental mechanics implemented and functional with minor issues. Core gameplay loop is intact. Game is published to itch.io.                                                                                                                                                                                                                                                                      |
| C    | 5-6.4   | Core mechanics present but several are incomplete or buggy. Player/enemy movement and combat work, but features such as pickups, high scores, sound effects or menus may be missing or partially functional. Game may be published to itch.io.                                                                                                                                                            |
| D/E  | 0-4.9   | Many fundamental mechanics missing or non-functional. Game is unplayable or fails to demonstrate the required features. May not be published to itch.io.                                                                                                                                                                                                                                                  |

#### Tier 2 Mechanics (4 marks)

| Band | Marks   | Criteria                                                                                                                                          |
| ---- | ------- | ------------------------------------------------------------------------------------------------------------------------------------------------- |
| A    | 3.2-4   | All three selected Tier 2 mechanics fully implemented and working seamlessly within the game. Mechanics enhance gameplay and are well-integrated. |
| B    | 2.6-3.1 | Three Tier 2 mechanics implemented with minor issues. Mechanics are functional and mostly integrated well.                                        |
| C    | 2-2.5   | Fewer than three Tier 2 mechanics implemented, or three present but with notable bugs or poor integration.                                        |
| D/E  | 0-1.9   | Fewer than two Tier 2 mechanics implemented, or mechanics are largely non-functional.                                                             |

#### Tier 3 Mechanics (3 marks)

| Band | Marks   | Criteria                                                                                                                         |
| ---- | ------- | -------------------------------------------------------------------------------------------------------------------------------- |
| A    | 2.4-3   | Both selected Tier 3 mechanics fully implemented and working seamlessly. Mechanics add meaningful depth and are well-integrated. |
| B    | 2-2.3   | Both Tier 3 mechanics implemented with minor issues. Mechanics are functional and reasonably integrated.                         |
| C    | 1.5-1.9 | Only one Tier 3 mechanic fully implemented, or both present but with significant issues or poor integration.                     |
| D/E  | 0-1.4   | Tier 3 mechanics largely missing or non-functional.                                                                              |

#### Your Own Mechanics (3 marks)

| Band | Marks   | Criteria                                                                                                                                                      |
| ---- | ------- | ------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| A    | 2.4-3   | Four original mechanics implemented that are clearly distinct from all listed mechanics. Each mechanic adds meaningful gameplay value and is well-integrated. |
| B    | 2-2.3   | Four original mechanics present with minor issues, or three fully implemented. Mechanics are generally distinct and add value.                                |
| C    | 1.5-1.9 | Fewer than four original mechanics, or mechanics closely resemble listed ones. Some integration issues.                                                       |
| D/E  | 0-1.4   | Fewer than two original mechanics implemented, or mechanics are not meaningfully distinct or functional.                                                      |

---

### Platformer (15 marks)

#### Fundamental Mechanics (7 marks)

| Band | Marks   | Criteria                                                                                                                                                                                                                                                                                                                                                                                              |
| ---- | ------- | ----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| A    | 5.6-7   | All fundamental mechanics fully implemented and functional. Main menu, player movement (A/D/Space via Unity input system), gravity, collisions, platform variety, enemy patrol and damage, score system with display, 3-life system with display, top 10 high scores, sound effects for all required events, pause/resume (P key) and end game menu all work correctly. Game is published to itch.io. |
| B    | 4.5-5.5 | Most fundamental mechanics implemented and functional with minor issues. Core gameplay loop is intact. Game is published to itch.io with most features accessible.                                                                                                                                                                                                                                    |
| C    | 3.5-4.4 | Core mechanics present but some are incomplete or buggy. Movement, gravity, collisions and basic score/lives system work, but features such as enemy AI, high scores, sound effects or menus may be missing or partially functional. Game may be published to itch.io.                                                                                                                                |
| D/E  | 0-3.4   | Many fundamental mechanics missing or non-functional. Game is unplayable or fails to demonstrate the required features. May not be published to itch.io.                                                                                                                                                                                                                                              |

#### Tier 2 Mechanics (3 marks)

| Band | Marks   | Criteria                                                                                                                                          |
| ---- | ------- | ------------------------------------------------------------------------------------------------------------------------------------------------- |
| A    | 2.4-3   | All three selected Tier 2 mechanics fully implemented and working seamlessly within the game. Mechanics enhance gameplay and are well-integrated. |
| B    | 2-2.3   | Three Tier 2 mechanics implemented with minor issues. Mechanics are functional and mostly integrated well.                                        |
| C    | 1.5-1.9 | Fewer than three Tier 2 mechanics implemented, or three present but with notable bugs or poor integration.                                        |
| D/E  | 0-1.4   | Fewer than two Tier 2 mechanics implemented, or mechanics are largely non-functional.                                                             |

#### Tier 3 Mechanics (3 marks)

| Band | Marks   | Criteria                                                                                                                         |
| ---- | ------- | -------------------------------------------------------------------------------------------------------------------------------- |
| A    | 2.4-3   | Both selected Tier 3 mechanics fully implemented and working seamlessly. Mechanics add meaningful depth and are well-integrated. |
| B    | 2-2.3   | Both Tier 3 mechanics implemented with minor issues. Mechanics are functional and reasonably integrated.                         |
| C    | 1.5-1.9 | Only one Tier 3 mechanic fully implemented, or both present but with significant issues or poor integration.                     |
| D/E  | 0-1.4   | Tier 3 mechanics largely missing or non-functional.                                                                              |

#### Your Own Mechanics (2 marks)

| Band | Marks   | Criteria                                                                                                                                                       |
| ---- | ------- | -------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| A    | 1.6-2   | Three original mechanics implemented that are clearly distinct from all listed mechanics. Each mechanic adds meaningful gameplay value and is well-integrated. |
| B    | 1.3-1.5 | Three original mechanics present with minor issues, or two fully implemented. Mechanics are generally distinct and add value.                                  |
| C    | 1-1.2   | Fewer than three original mechanics, or mechanics closely resemble listed ones. Some integration issues.                                                       |
| D/E  | 0-0.9   | Fewer than two original mechanics implemented, or mechanics are not meaningfully distinct or functional.                                                       |

---

## Code Quality and Best Practices - Learning Outcome 1 (40 marks)

---

### Project Structure (8 marks)

| Band | Marks   | Criteria                                                                                                                                                                                                                                                                                                                     |
| ---- | ------- | ---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| A    | 6.4-8   | Excellent project structure across all three games. Clear and logical organisation with well-defined separation of concerns. Scripts, assets, scenes and prefabs are meaningfully separated and named. Code is highly modular, promoting reusability and maintainability. Easy to navigate and follows Unity best practices. |
| B    | 5.2-6.3 | Good project structure with separation of concerns implemented for most games. Organisation is logical with minor inconsistencies. Code is reasonably modular with some reusable components. Generally follows best practices.                                                                                               |
| C    | 4-5.1   | Basic structure present but separation of concerns is inconsistent. Some mixing of responsibilities within scripts or scenes. Limited modularity. Organisation could be significantly improved across one or more games.                                                                                                     |
| D/E  | 0-3.9   | Poor or no consistent project structure. Little to no separation of concerns. Code is monolithic and difficult to navigate across one or more games.                                                                                                                                                                         |

---

### Naming Conventions (6 marks)

| Band | Marks   | Criteria                                                                                                                                                                                                                                                  |
| ---- | ------- | --------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| A    | 4.8-6   | All file, class, method, variable, constant and property names are clear, descriptive and consistent across all three games. Naming conventions are applied correctly and uniformly throughout the codebase. Names immediately convey purpose and intent. |
| B    | 3.9-4.7 | Names are generally clear and descriptive with minor inconsistencies. Naming conventions are followed in most cases across the codebase.                                                                                                                  |
| C    | 3-3.8   | Naming is inconsistent or unclear in places. Some names are not descriptive or do not follow a consistent convention. Issues spread across one or more games.                                                                                             |
| D/E  | 0-2.9   | Poor naming conventions throughout. Many names are unclear, abbreviated or inconsistent. Makes the codebase difficult to understand.                                                                                                                      |

---

### Documentation and Comments (10 marks)

| Band | Marks   | Criteria                                                                                                                                                                                                                                                                                   |
| ---- | ------- | ------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------ |
| A    | 8-10    | Codebase is comprehensively documented using the XML documentation format across all three games. All classes and public methods have XML doc comments. Complex logic is clearly explained with inline comments. Comments add genuine value and clarify intent rather than restating code. |
| B    | 6.5-7.9 | Good documentation coverage using XML format for most classes and methods. Complex logic is mostly commented. Minor gaps in documentation across the codebase.                                                                                                                             |
| C    | 5-6.4   | Basic documentation present but inconsistent. XML format used in some places but not throughout. Some complex logic is unexplained. One or more games may have limited documentation.                                                                                                      |
| D/E  | 0-4.9   | Little or no documentation. XML format largely absent. Code is difficult to understand without explanation.                                                                                                                                                                                |

---

### Code Formatting (6 marks)

| Band | Marks   | Criteria                                                                                                                                                                                                                 |
| ---- | ------- | ------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------ |
| A    | 4.8-6   | Consistent indentation and spacing used throughout all three games. Proper indentation for all code blocks, consistent brace formatting and appropriate blank lines between sections. Code is clean and highly readable. |
| B    | 3.9-4.7 | Formatting is consistent in most areas with minor lapses. Indentation and spacing are generally correct. Code is readable.                                                                                               |
| C    | 3-3.8   | Formatting is inconsistent across the codebase. Some indentation errors, inconsistent brace styles or missing blank lines. Readability is impacted.                                                                      |
| D/E  | 0-2.9   | Little or no consistent formatting. Indentation, spacing and structure are irregular throughout. Code is difficult to read.                                                                                              |

---

### Dead Code (4 marks)

| Band | Marks   | Criteria                                                                                                                                                          |
| ---- | ------- | ----------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| A    | 3.2-4   | No unused or redundant files, classes, methods, variables, constants or properties across all three games. Codebase is clean and contains only what is necessary. |
| B    | 2.6-3.1 | Minimal dead code present. A few unused elements exist but do not significantly clutter the codebase.                                                             |
| C    | 2-2.5   | Some dead code present across one or more games. Unused variables, commented-out blocks or redundant files are noticeable.                                        |
| D/E  | 0-1.9   | Significant dead code throughout the codebase. Unused classes, methods or files are widespread and create confusion.                                              |

---

### Performance and Scalability (6 marks)

| Band | Marks   | Criteria                                                                                                                                                                                                                                                                     |
| ---- | ------- | ---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| A    | 4.8-6   | Code is well-optimised across all three games. Efficient algorithms and appropriate data structures are used throughout. No obvious performance bottlenecks. Unity best practices (e.g., caching component references, avoiding per-frame allocations) consistently applied. |
| B    | 3.9-4.7 | Code is generally efficient with minor performance concerns. Most Unity best practices applied. Some suboptimal patterns present but not significantly impactful.                                                                                                            |
| C    | 3-3.8   | Basic functionality is achieved but with noticeable performance issues or inefficient approaches. Some Unity best practices applied inconsistently.                                                                                                                          |
| D/E  | 0-2.9   | Code has significant performance issues. Inefficient algorithms or poor data structures used. Little evidence of performance awareness or Unity best practices.                                                                                                              |

---

## Documentation and Git Usage - Learning Outcome 1 (5 marks)

---

### GitHub Issues (2 marks)

| Band | Marks   | Criteria                                                                                                                                                                                                                                                       |
| ---- | ------- | -------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| A    | 1.6-2   | Consistent and meaningful use of GitHub Issues throughout the full duration of the assessment. Issues are well-defined, used to organise and prioritise development work and shown to the course lecturer both before development and at the end of each week. |
| B    | 1.3-1.5 | Good use of GitHub Issues with mostly consistent engagement. Issues are present and generally reflect development progress. Shown at most required checkpoints.                                                                                                |
| C    | 1-1.2   | Basic use of GitHub Issues but inconsistent. Some issues created but do not clearly reflect development priorities or were not shown at required checkpoints.                                                                                                  |
| D/E  | 0-0.9   | Little or no use of GitHub Issues. Development work is not tracked or organised through issues.                                                                                                                                                                |

---

### README.md (1 mark)

| Band | Marks   | Criteria                                                                                                                                                                                                                                              |
| ---- | ------- | ----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| A    | 0.8-1   | README.md is complete and well-formatted. Includes working itch.io URLs for all three games and a clear list of implemented mechanics for each game. Any AI tool usage or external code sources are fully acknowledged with prompts and descriptions. |
| B    | 0.7     | README.md covers all required sections with minor omissions. itch.io URLs and mechanic lists are present. AI/external code acknowledgement present but may lack detail.                                                                               |
| C    | 0.5-0.6 | README.md is incomplete. Some URLs or mechanic lists are missing. AI/external code acknowledgement may be absent or insufficient.                                                                                                                     |
| D/E  | 0-0.4   | README.md is largely absent or missing critical sections. itch.io URLs and/or mechanics lists not provided.                                                                                                                                           |

---

### .gitignore (1 mark)

| Band | Marks   | Criteria                                                                                                                                        |
| ---- | ------- | ----------------------------------------------------------------------------------------------------------------------------------------------- |
| A    | 0.8-1   | A correct Unity .gitignore is present and based on the provided resource. No unnecessary Unity-generated files are committed to the repository. |
| B    | 0.7     | .gitignore is present and functional with minor omissions. Most unnecessary files are excluded.                                                 |
| C    | 0.5-0.6 | .gitignore is present but incomplete or not based on the Unity template. Some generated files are committed unnecessarily.                      |
| D/E  | 0-0.4   | .gitignore is missing or incorrect. Large numbers of generated Unity files are committed to the repository.                                     |

---

### Git Commit Messages (1 mark)

| Band | Marks   | Criteria                                                                                                                                                                                                                           |
| ---- | ------- | ---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| A    | 0.8-1   | Commit messages are clear, descriptive and consistently follow an appropriate naming convention. Each commit reflects a focused, meaningful change. Regular commits demonstrate ongoing development. No large, monolithic commits. |
| B    | 0.7     | Commit messages are generally clear and follow a naming convention with minor inconsistencies. Commits are reasonably regular and focused.                                                                                         |
| C    | 0.5-0.6 | Commit messages vary in quality. Some are vague or do not reflect the changes made. Commits may be infrequent or inconsistently sized.                                                                                             |
| D/E  | 0-0.4   | Commit messages are unclear, missing or do not follow any convention. Very few commits or large monolithic commits indicate poor version control habits.                                                                           |

---

## Mark Summary

| Section                            | Marks Available |
| ---------------------------------- | --------------- |
| Breakout – Fundamental Mechanics   | 7               |
| Breakout – Tier 2 Mechanics        | 3               |
| Breakout – Tier 3 Mechanics        | 3               |
| Breakout – Your Own Mechanics      | 2               |
| RogueLike – Fundamental Mechanics  | 10              |
| RogueLike – Tier 2 Mechanics       | 4               |
| RogueLike – Tier 3 Mechanics       | 3               |
| RogueLike – Your Own Mechanics     | 3               |
| Platformer – Fundamental Mechanics | 7               |
| Platformer – Tier 2 Mechanics      | 3               |
| Platformer – Tier 3 Mechanics      | 3               |
| Platformer – Your Own Mechanics    | 2               |
| **Functionality Total**            | **50**          |
| Project Structure                  | 8               |
| Naming Conventions                 | 6               |
| Documentation and Comments         | 10              |
| Code Formatting                    | 6               |
| Dead Code                          | 4               |
| Performance and Scalability        | 6               |
| **Code Quality Total**             | **40**          |
| GitHub Issues                      | 2               |
| README.md                          | 1               |
| .gitignore                         | 1               |
| Git Commit Messages                | 1               |
| **Documentation and Git Total**    | **5**           |
| **Total**                    | **100**         |

---

_Author: Grayson Orr_  
_Course: ID623002: Introductory Game Development_