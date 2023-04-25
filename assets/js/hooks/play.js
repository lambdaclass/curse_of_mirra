import {Player} from "../game/player.js"
import { time_now } from "../time_utils.js"


export const Play = function () {
    this.mounted = function () {
        let game_id = document.getElementById("board_game").dataset.gameId
        let player = new Player(game_id)
        this.player = player
        now = time_now()
        this.last_updated = now

        document.addEventListener("keypress", function onPress(event) {
            console.log(event.key);
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
            if (event.key === " "){
                player.attack()
            }
            if (event.key === "k"){
                player.createAoeAttack()
            }
        }
        );

        document.addEventListener("keyup", function onPress(event) {
            console.log(event.key);
            if (event.key === "k"){
                console.log("depressed K")
                player.activateAoeAttack()
            }
        }
        );
    }

    this.updated = function () {
        now = time_now()

        let last_updated_plus_one_second = new Date()
        last_updated_plus_one_second.setTime(this.last_updated)
        last_updated_plus_one_second.setSeconds(last_updated_plus_one_second.getSeconds() + 1)

        // Only update ping if more than one second has passed between the last update.
        if (now > last_updated_plus_one_second) {
            this.last_updated = now
            this.player.ping();
        }
    }
}
