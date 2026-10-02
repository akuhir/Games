extends CanvasLayer

@onready var result_label: Label = $Panel/ResultLabel
@onready var restart_button: Button = $Panel/RestartButton

func _ready() -> void:
	visible = false
	LapManager.race_finished.connect(_on_race_finished)
	restart_button.pressed.connect(_on_restart_pressed)

func _on_race_finished(total_time: float) -> void:
	var minutes := int(total_time) / 60
	var seconds := int(total_time) % 60
	var millis := int((total_time - int(total_time)) * 100)
	result_label.text = "Race Complete!\nTime: %02d:%02d.%02d" % [minutes, seconds, millis]
	visible = true
	get_tree().paused = true

func _on_restart_pressed() -> void:
	get_tree().paused = false
	get_tree().reload_current_scene()
