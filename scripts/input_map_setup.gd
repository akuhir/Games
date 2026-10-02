extends Node
# Autoload: registers keyboard input actions at startup so the same
# Input.get_axis() calls in car.gd work for keyboard (desktop testing)
# AND for the on-screen TouchScreenButton nodes (mobile), since those
# buttons trigger these same action names directly.

func _init() -> void:
	_add_key_action("accelerate", KEY_W)
	_add_key_action("accelerate", KEY_UP)
	_add_key_action("brake", KEY_S)
	_add_key_action("brake", KEY_DOWN)
	_add_key_action("steer_left", KEY_A)
	_add_key_action("steer_left", KEY_LEFT)
	_add_key_action("steer_right", KEY_D)
	_add_key_action("steer_right", KEY_RIGHT)

func _add_key_action(action_name: String, keycode: int) -> void:
	if not InputMap.has_action(action_name):
		InputMap.add_action(action_name)
	var ev := InputEventKey.new()
	ev.physical_keycode = keycode
	InputMap.action_add_event(action_name, ev)
