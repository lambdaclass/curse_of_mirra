export MIX_ENV=prod
cd /tmp
# Clone and compile the game.
git clone https://github.com/lambdaclass/curse_of_myrra.git dark_worlds_server
cd dark_worlds_server/
git sparse-checkout set --no-cone server
git checkout
cd server/

mix local.hex --force && mix local.rebar --force
mix deps.get --only $MIX_ENV
mix deps.compile
mix assets.deploy
mix compile
mix phx.gen.release
mix release

rm -rf $USER/dark_worlds_server
mv /tmp/dark_worlds_server $HOME/

# Create a service for the gmae.
cat <<EOF > /etc/systemd/system/dark_worlds_server.service
[Unit]
Description=Dark Worlds server
Requires=network-online.target
After=network-online.target

[Service]
User=root
WorkingDirectory=$HOME/dark_worlds_server/server
Restart=on-failure
ExecStart=$HOME/dark_worlds_server/server/entrypoint.sh
ExecReload=/bin/kill -HUP
KillSignal=SIGTERM
EnvironmentFile=/root/.env

[Install]
WantedBy=multi-user.target
EOF
