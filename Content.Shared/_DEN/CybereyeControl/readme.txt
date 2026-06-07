Do not port anything that's in this folder, this is a bandaid fix!
turns out there is no easy way to do this propely.
the cleanest approach would be some sort of interface deal where every one of the ShowXComponent comps are handled by one thing, but that requires essentially refactoring how EquipmentHudSystem works
I made all those components inherit from one base component but that's about it for actual coding
the logic flow would be something like:
collect all of the hud components on the entity
open a radial menu, populate it with those components as options
make picking options in the menu enable/disable the individual hud  systems
all of this is code I've never personally touched and I honestly have no clue where to even start looking for how to do this

the way its done here is super bad don't do this ever.
it's prebase so who cares, I would not do this normally but I'm losing my mind trying to understand how I can store a component from an entity in a component registry and I can't anymore