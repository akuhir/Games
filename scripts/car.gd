extends CharacterBody3D

@export var acceleration := 18.0
@export var max_speed := 35.0
@export var braking := 30.0
@export var steering_speed := 2.5
@export var friction := 8.0
@export var gravity := 20.0

var throttle := 0.0
var steering := 0.0

func _ready() -> void:
	# Lets Checkpoint areas identify the car via body_entered.
	add_to_group("player_car")

func _physics_process(delta: float) -> void:
	# Reads "accelerate"/"brake"/"steer_left"/"steer_right".
	# Keyboard keys are bound in input_map_setup.gd for desktop testing;
	# the on-screen TouchScreenButton nodes in TouchControls.tscn trigger
	# these exact same action names, so no separate mobile code path
	# is needed here.
	throttle = Input.get_axis("brake", "accelerate")
	steering = Input.get_axis("steer_left", "steer_right")

	# Keep the car glued to the ground / apply gravity when airborne.
	if not is_on_floor():
		velocity.y -= gravity * delta
	else:
		velocity.y = 0.0

	# Acceleration
	if throttle > 0:
		velocity.z = move_toward(
			velocity.z,
			-max_speed,
			acceleration * delta
		)

	# Braking / reverse
	elif throttle < 0:
		velocity.z = move_toward(
			velocity.z,
			max_speed * 0.25,
			braking * delta
		)

	# Friction
	else:
		velocity.z = move_toward(
			velocity.z,
			0,
			friction * delta
		)

	# Steering (only while actually moving)
	if abs(velocity.z) > 0.5:
		rotation.y -= steering * steering_speed * delta

	# Convert the scalar forward speed stored in velocity.z into a
	# world-space velocity along the car's current facing direction.
	var forward := -global_transform.basis.z
	var speed := -velocity.z
	velocity.x = forward.x * speed
	velocity.z = forward.z * speed

	move_and_slide()
