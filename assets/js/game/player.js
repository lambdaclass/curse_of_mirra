import { time_now } from "../time_utils.js"

export class Player{
    constructor(game_id) {
        this.socket = new WebSocket(this.getplayConnection(game_id));

        this.socket.addEventListener("message", (event) => {
            if (event.data == "pong") {
                let now = time_now();
                let latency = now - this.last_ping_sent;
                this.socket.send(this.createLatencyMessage(latency))
            }
        });

        this.aimingDirection = "right"; // Set up a default direction
    }

    move(direction) {
        this.aimingDirection = direction;
        this.socket.send(this.createMoveMessage(direction))
    }

    attack() {
        this.socket.send(this.createAttackMessage())
    }


    createAoeAttack() {
        this.socket.send(this.createAoeAttackMessage(this.aimingDirection))
    }

    activateAoeAttack() {
        this.socket.send(this.createAoeAttackMessage("activate"))
    }

    createAttackMessage(){
        let msg = {
            action: "attack",
            value: this.aimingDirection
        }

        return JSON.stringify(msg)
    }

    /*
    fn attack_aoe(
        game: GameState,
        attacking_player_id: u64,
        direction: Direction,
        keypressed: bool,
    )
    */
    createAoeAttackMessage(direction){
        let msg = {
            action: "attack_aoe",
            value: direction,
        }

        return JSON.stringify(msg)
    }

    

    createMoveMessage(direction) {
        let msg = {
            action: "move",
            value: direction
        }

        return JSON.stringify(msg)
    }

    getplayConnection(game_id) {
        let protocol = window.location.protocol === 'https:' ? 'wss:' : 'ws:'
        let host = window.location.host
        let path = '/play'

        return `${protocol}${host}${path}/${game_id}`
    }

    createLatencyMessage(latency) {
        let msg = {
            action: "update_ping",
            value: latency
        }

        return JSON.stringify(msg)
    }

    ping() {
        let msg = { action: "ping", value: "ping" }

        now = time_now()
        this.last_ping_sent = now;

        this.socket.send(JSON.stringify(msg))
    }
}
