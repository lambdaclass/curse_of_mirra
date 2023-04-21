import {Player} from "../game/player.js"
import { update_ping } from "../ping.js"

export const Play = function () {
    this.mounted = function () {
        let game_id = document.getElementById("board_game").dataset.gameId
        let player = new Player(game_id)
        update_ping(this)
        now = time_now()
        this.last_updated = now

        document.addEventListener("keypress", function onPress(event) {
            if (event.key === "a") {
                player.move("left")
            }
            if (event.key === "w") {
                player.move("up")
            }
            if (event.key === "s") {
                player.move("down")
            }
            if (event.key === "d") {
                player.move("right")
            }
        });
    }

    this.updated = function () {
        now = time_now()

        let last_updated_plus_one_second = new Date()
        last_updated_plus_one_second.setTime(this.last_updated)
        last_updated_plus_one_second.setSeconds(last_updated_plus_one_second.getSeconds() + 1)

        if (now > last_updated_plus_one_second) {
            this.last_updated = now
            update_ping(this)
        }
    }
}

function time_now() {
    const date = new Date();
    let time = date.getTime();
    date.setTime(time);

    return date;
}


