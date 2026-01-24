using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Threading.Tasks;
using System.Windows.Forms;
#nullable enable

namespace PrisonerDilemma
{
    internal class Games
    {
        public event EventHandler? RoundPlayed;
        readonly private Agent[,] agents;
        readonly private CheckBox torroidalField;
        private Game game;
        
        public Games(Agent[,] PAgents, CheckBox PTorroidalFieldCbox, Game PGame)
        {
            agents = PAgents;
            game = PGame;
            torroidalField = PTorroidalFieldCbox;
        }

        internal void PlayRound(object PSender, EventArgs PE)
        {
            int nx = agents.GetLength(0);
            int ny = agents.GetLength(1);

            initAgents(nx, ny); // Initialise the agents for the round

            if (torroidalField.Checked)
            {   
                playTorroidal(nx, ny);  // Play with toroidal wrapping
            }
            else
            {   
                playNoWrapping(nx, ny); // Play without wrapping
            }
            
            finaliseRound(nx, ny);      // Finalise the round for all agents

            RoundPlayed?.Invoke(this, EventArgs.Empty);
        }


        private void initAgents(int PNx, int PNy)
        {   // Initialise all agents for the round
            for (int ix = 0; ix < PNx; ix++)
            {
                for (int iy = 0; iy < PNy; iy++)
                {
                    Agent currentAgent = agents[ix, iy];
                    currentAgent.InitRound();
                }
            }
        }

        private void playTorroidal(int PNx, int PNy)
        {   // Play neighbours E, SE, S and SW
            for (int ix = 0; ix < PNx; ix++)
            {
                for (int iy = 0; iy < PNy; iy++)
                {
                    Agent currentAgent = agents[ix, iy];
                    Agent eNeighbour = agents[(ix + 1) % PNx, iy];
                    Agent sNeighbour = agents[ix, (iy + 1) % PNy];
                    Agent swNeighbour = agents[(ix - 1 + PNx) % PNx, (iy + 1) % PNy];
                    Agent seNeighbour = agents[(ix + 1) % PNx, (iy + 1) % PNy];
                    game.Play(currentAgent, eNeighbour);
                    game.Play(currentAgent, sNeighbour);
                    game.Play(currentAgent, seNeighbour);
                    game.Play(currentAgent, swNeighbour);
                }
            }
        }
        
        private void playNoWrapping(int PNx, int PNy)
        {   // Play without wrapping
            for (int ix = 0; ix < PNx; ix++)
            {
                for (int iy = 0; iy < PNy; iy++)
                {   // Play neighbours E, SE, S and SW
                    bool eOk = (ix < PNx - 1);
                    bool sOk = (iy < PNy - 1);
                    bool wOk = (ix > 0);
                    Agent currentAgent = agents[ix, iy];
                    if (eOk)
                    {
                        Agent eNeighbour = agents[ix + 1, iy];
                        game.Play(currentAgent, eNeighbour);
                    }
                    if (sOk)
                    {
                        Agent sNeighbour = agents[ix, iy + 1];
                        game.Play(currentAgent, sNeighbour);

                        if (eOk)
                        {
                            Agent seNeighbour = agents[ix + 1, iy + 1];
                            game.Play(currentAgent, seNeighbour);
                        }
                        if (wOk)
                        {
                            Agent swNeighbour = agents[ix - 1, iy + 1];
                            game.Play(currentAgent, swNeighbour);
                        }
                    }
                }
            }
        }

        private void finaliseRound(int PNx, int PNy)
        {
            for (int ix = 0; ix < PNx; ix++)
            {
                for (int iy = 0; iy < PNy; iy++)
                {
                    Agent currentAgent = agents[ix, iy];
                    currentAgent.FinaliseRound();
                }
            }
        }

    }
}
