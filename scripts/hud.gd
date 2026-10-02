extends CanvasLayer

@onready var lap_label: Label = $LapLabel
@onready var time_label: Label = $TimeLabel

func _ready() -> void:
	LapManager.lap_completed.connect(_on_lap_completed)
	_update_lap_label()

func _process(_delta: float) -> void:
	time_label.text = "Time: %s" % _format_time(LapManager.race_time)

func _on_lap_completed(_lap: int) -> void:
	_update_lap_label()

func _update_lap_label() -> void:
	lap_label.text = "Lap: %d / %d" % [LapManager.lap, LapManager.laps_to_win]

func _format_time(t: float) -> String:
	var minutes := int(t) / 60
	var seconds := int(t) % 60
	var millis := int((t - int(t)) * 100)
	return "%02d:%02d.%02d" % [minutes, seconds, millis]
