# Usage: ./setup_game_server.sh <BRANCH_NAME>
# If no BRANCH_NAME is provided, defaults to main

BRANCH_NAME="$1"
BRANCH_NAME=${BRANCH_NAME:-"main"}

export MIX_ENV=prod
cd /tmp
# # Clone and compile the game.
git clone https://github.com/lambdaclass/curse_of_myrra.git curse_of_myrra
cd curse_of_myrra/
git sparse-checkout set --no-cone server
git checkout $BRANCH_NAME
cd server/

mix local.hex --force && mix local.rebar --force
mix deps.get --only $MIX_ENV
mix deps.compile
mix assets.deploy
mix compile
mix phx.gen.release
mix release

rm -rf $USER/curse_of_myrra
mv /tmp/curse_of_myrra $HOME/

# Create a service for the gmae.
cat <<EOF > /etc/systemd/system/curse_of_myrra.service
[Unit]
Description=Curse Of Myrra server
Requires=network-online.target
After=network-online.target

[Service]
User=root
WorkingDirectory=$HOME/curse_of_myrra/server
Restart=on-failure
ExecStart=$HOME/curse_of_myrra/server/entrypoint.sh
ExecReload=/bin/kill -HUP
KillSignal=SIGTERM
EnvironmentFile=/root/.env

[Install]
WantedBy=multi-user.target
EOF
