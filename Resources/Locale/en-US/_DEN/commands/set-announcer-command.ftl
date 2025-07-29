cmd-set-announcer-help = Usage: {$command} <announcerID: string> [announceChange: boolean = true] [reason: string]
cmd-set-announcer-desc = Sets the active announcer for the current round.

cmd-set-announcer-error-invalid-announcer-id = Invalid value given for announcerID (should be valid announcer ID)!
cmd-set-announcer-error-invalid-announce-change = Invalid value given for announceChange (should be boolean)!
cmd-set-announcer-error-invalid-reason = Invalid value given for reason (should be string)!
cmd-set-announcer-error-arg-count = Invalid number of arguments passed to command! Expected between {$min} and {$max} arguments.
cmd-set-announcer-error-same-announcer = Announcer is already set to {$announcer}!

cmd-set-announcer-error-list-announcers = Valid announcer IDs: {$announcers}
cmd-set-announcer-success = Current announcer set to {$announcer}.
