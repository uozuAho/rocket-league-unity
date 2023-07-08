# My Rocket League clone

# to do
- handbrake turn
  - hard!
  - to try
    - model individual wheel slip & forces when handbrake
      is pressed. Non-physics turning may be messing with torques/forces
        - need to figure out how parenting/joints are supposed to work
    - may need to revisit wheel collider
        - maybe switch to WC physics only when handbrake is pressed?
  - failed attempts
    - reduce friction at rear wheels using AddForceAtPosition,
      cause weird magnet-like spin/locking behaviour. I dunno why.
- prevent getting stuck in walls when crashing at high speed
- forward/backward pitch in air
- flip
- boost
- air roll
- prevent bouncing when hitting the ground on wheels
- make curve between ground + walls
  - need more modelling exp
- add roof
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
