extends Node
# Autoload singleton. Tracks checkpoint order, lap count, and race time.

signal lap_completed(lap_number: int)
signal race_finished(total_time: float)
signal checkpoint_reached(index: int)

@export var total_checkpoints := 6
@export var laps_to_win := 3

var current_expected_checkpoint := 0
var lap := 0
var race_time := 0.0
var race_started := false
var race_over := false
var has_crossed_start := false

func _process(delta: float) -> void:
	if race_started and not race_over:
		race_time += delta

func start_race() -> void:
	race_started = true
	race_time = 0.0
	lap = 0
	current_expected_checkpoint = 0
	has_crossed_start = false
	race_over = false

func checkpoint_passed(index: int) -> void:
	if race_over:
		return
	if not race_started:
		start_race()
	# Ignore gates hit out of order so players can't skip the course.
	if index != current_expected_checkpoint:
		return

	checkpoint_reached.emit(index)

	if index == 0:
		if has_crossed_start:
			lap += 1
			lap_completed.emit(lap)
			if lap >= laps_to_win:
				race_over = true
				race_finished.emit(race_time)
		has_crossed_start = true

	current_expected_checkpoint = (index + 1) % total_checkpoints
