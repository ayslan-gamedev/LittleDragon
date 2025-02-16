extends CharacterBody2D

var speed = 400  # speed in pixels/sec

func _ready():
	add_to_group("player")
	pass

func _physics_process(delta):
	var direction = Input.get_vector("ui_left", "ui_right", "ui_up", "ui_down")
	velocity = direction * speed

	move_and_slide()