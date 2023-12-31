# My Rocket League clone

Just a project to learn Unity. Work in progress. Controls are with a gamepad,
similar to standard rocket league controls.

![](./img/gameplay.png)

# setup
- install Unity 2022.3 LTS
- clone this repo
- open the repo directory with unity hub
- open the 'Game' scene in the project window
- click play
- have fun!

# to do
- flip
    - implement other directions
    - how does flip boost relate to car & joystick position?
    - play with force/drag
    - cancel vertical velocity?
- switch between ball/car cam
- air drag: max in air speed is too high
- increase turn speed at lower car speeds
- right stick cam look
- prevent getting stuck in walls when crashing at high speed
- flip car when on back
- prevent bouncing when hitting the ground on wheels
- make curve between ground + walls
  - need more modelling exp
- add roof
- fix ballcam to keep car in view
- visual: steer wheels
- visual: spin wheels when moving
- hold jump increases jump height

# notes on RL cam behaviour
- follower cam: car in same spot on screen, slightly below middle.
  While car is on ground, stay behind car. When car not on ground, stay
  at same horizontal rotation as when car left the ground.
- ball cam:
  - always point at ball
  - cam distance from car is always the same
  - cam position is always opposite the ball
  - camera pitch threshold: keep level until ball is level with car (?)
  - todo: what does cam do when ball is above car and car is on ground?

# maybe/later
- do arcade car
- wheel collider car
  - pain in the arse to get good settings
    - I don't understand the friction model. How to eliminate slip?
  - find good torque/friction etc settings
    - default values: too slippery
    - high stiffness: very jittery
    - ref:
      - https://docs.unity3d.com/Manual/WheelColliderTutorial.html
      - https://docs.unity3d.com/ScriptReference/WheelCollider.html
  - cam angle calculation isn't right
