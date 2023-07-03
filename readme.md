# My Rocket League clone

# doing
- do custom car physics
  - follower cam
    - get behaviour from RL: follower + ball cam
      - follower cam: keep car in same spot on screen, slightly below middle.
        While car is on ground, stay behind car. When car not on ground, stay
        at same horizontal rotation as when car left the ground.
      - ball cam: always point at ball. Car should always be in same spot
        on screen: slightly below middle
  - in air yaw
    - max angular speed
    - faster accel
  - only one double jump
  - prevent bouncing when hitting the ground on wheels

# todo
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
