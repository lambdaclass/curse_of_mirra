## Load Testing guide
The goal of load testing is to simulate a real scenario where there
are a lot of players in one server, our current goal is 1000 players by server.

Our current methodology is to use 2 servers to load test:
- The game server: where the game's server is actually running.
- The load test client: sends requests to the game server, we 
  want a dedicated load test client to avoid consumer network
  and hardware limitations.

Soem things to keep in mind about load tests:
- Always write a report, and every report must go to:
  server/load_test/reports/your_report.md, this is important 
  to keep track of possible performance regressions.
- Always write down the specs + OS + config from where you're running the tests.
  For example, did you change linux's governor settings? Write it down.
  Did you test any BEAM VM flag? Keep a record of it.
- Write down every snippet code that you've used, be it Elixir, Rust or bash commands,
  if you think it's obvious, write it down just in case.
- Track records of the parameters you've used for the tests, eg: Player Amount,
  where there bots enabled? etc.
- Use text or pictures for reports, videos only if there are UX (i.e. gameplay) issues.
- Use multiple sources of truth and data, like htop, New Relic, erlang's etop/fprop.
- Feel free to experiment a bit, certain VM flags can improve or hinder performance,
  if you find improvements.
- It's important for load tests to be reproducible.

### Setup
I recommend you add each server ip to your ~/.ssh/config file to avoid confusions, like this:
```conf 
Host myrra_load_test_client
  Hostname client_ip

Host myrra_load_test_server
  Hostname game_ip
```

### Game Server Setup
1. Log into it with ssh: 
   ```sh
   ssh myuser@myrra_load_test_server
   ```
2. If it's not already there, copy the script on this repo under
   `server/load_test/setup_game_server.sh` it clones the game server, compiles it, and creates a
   systemd service for it, run it with:
   ```sh
   chmod +x ./setup_game_server.sh && ./setup_game_server
   ```
4. Now you can start the game server with: 
```sh
   systemctl daemon-reload && systemctl start dark_worlds_server
```
   You can check the logs with `journalctl -xefu dark_worlds_server`.
5. Make sure to disable hyperthreading, if using an x86 CPU:
```sh
# If active, this returns 1
cat /sys/devices/system/cpu/smt/active
# Turn off hyperthreading
echo off | sudo tee /sys/devices/system/cpu/smt/control
```
One way of checking this, besides the command above,
is to open htop, you should see the virtual cores as 'offline'.

### Load Test Client setup
1. Log into it with ssh: 
   ```sh
   ssh myuser@myrra_load_test_client
   ```
2. If not already there, copy this repo's script under `server/load_test/setup_load_client.sh`
   and run it:
   ```sh
   chmod +x ./setup_load_client.sh && ./setup_load_client
   ```
3. Set this env variable: `export SERVER_HOST=game_server_ip:game_server_port`.
4. Run:
   ```sh
       ./dark_worlds_server/server/load_test/_build
   ``` 
   this drops you into an Elixir shell from which you'll run the load tests.
5. From the elixir shell, you can run: `LoadTest.PlayerSupervisor.spawn_players(NUMBER_OF_USERS, PLAY_TIME)` 
