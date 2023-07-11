## Benchmarking guide
For benchmarking, you should follow this steps:
1. Deploy your branch, obviously.
2. Once deployed, ssh into the server and disable hyperthreading:
```sh
# If active, this returns 1
cat /sys/devices/system/cpu/smt/active
# Turn off hyperthreading
echo off | sudo tee /sys/devices/system/cpu/smt/control
```
This step might require sudoing in first with `sudo -i`.

3. Before starting the load tests, check the following:
   - Server logs, for potential errors and crashes with `journalctl -xefu dark_worlds_server`.
   - Htop, for system usage.

   Also, before EACH load test, restart the running instance with
   `systemctl restart dark_worlds_server`
   You can use this tmux script to have a window for each item above:
  ```sh
#!/bin/bash
# Create a new session
tmux new-session -d -s mysession

# Open log window
tmux new-window -t mysession -n "Window 1"
tmux send-keys -t mysession:1 'journalctl -xefu dark_worlds_server' C-m

# Open htop window
tmux new-window -t mysession -n "Window 2"
tmux send-keys -t mysession:2 'htop' C-m

# Attach to the session to view the windows
tmux attach-session -t mysession
``` 
Switch between htop and logs with Ctrl-b 1 or Ctrl-b 2.

4. Locally, on your machine, then run the load tests 
   from the load_tests app. Run `iex -S mix` there. The function you want
   to call is `LoadTest.PlayerSupervisor.n_games_30_players/1`, which takes
   as an argument how many game sessions to start.

5. On the frontend, check for how good the user experience is! This is very important!
   If you can, try to play with someone else while the load tests run.

