all:
	ninja -C build
	meson --buildtype=debug . build -Ddbus=false
