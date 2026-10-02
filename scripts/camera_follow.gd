extends Camera3D

@export var target_path: NodePath
@export var follow_distance := 8.0
@export var follow_height := 4.0
@export var follow_speed := 5.0
@export var look_ahead := 3.0

var target: Node3D

func _ready() -> void:
	if target_path != NodePath():
		target = get_node(target_path)

func _physics_process(delta: float) -> void:
	if not target:
		return
	var back := target.global_transform.basis.z.normalized()
	var desired_pos := target.global_position + back * follow_distance + Vector3.UP * follow_height
	global_position = global_position.lerp(desired_pos, clamp(follow_speed * delta, 0.0, 1.0))
	var look_target := target.global_position + Vector3.UP * 1.0 - back * look_ahead
	look_at(look_target, Vector3.UP)
