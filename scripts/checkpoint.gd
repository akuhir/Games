extends Area3D

@export var checkpoint_index := 0

func _ready() -> void:
	body_entered.connect(_on_body_entered)

func _on_body_entered(body: Node3D) -> void:
	if body.is_in_group("player_car"):
		LapManager.checkpoint_passed(checkpoint_index)
