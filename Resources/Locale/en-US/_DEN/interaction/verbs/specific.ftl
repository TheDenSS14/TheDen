interaction-WashHands-name = Wash hands / item
interaction-WashHands-description = Wash your hands (or the item you're holding) in the water.

interaction-WashHands-success-self-popup = You wash {$hasUsed ->
    [false] your hands in {THE($target)}.
    *[true] {THE($used)} in {THE($target)}.
}

interaction-WashHands-success-target-popup = {THE($user)} washes {$hasUsed ->
    [false] {POSS-ADJ($user)} hands.
    *[true] {THE($used)}.
}

interaction-WashHands-success-others-popup = {THE($user)} washes {$hasUsed ->
    [false] {POSS-ADJ($user)} hands in {THE($target)}.
    *[true] {THE($used)} in {THE($target)}.
}
