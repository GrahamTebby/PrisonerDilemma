using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
#nullable enable

namespace PrisonerDilemma
{
    internal class Games
    {
        public event EventHandler? RoundPlayed;
        private Agent[,] agents;
        private CheckBox torroidalField;
        private Game game;
        
        public Games(Agent[,] PAgents, CheckBox PTorroidalFieldCbox, Game PGame)
        {
            agents = PAgents;
            game = PGame;
            torroidalField = PTorroidalFieldCbox;
        }

        internal void PlayRound(object sender, EventArgs e)
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

            
            FinaliseRound(nx, ny);      // Finalise the round for all agents

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
        {
            for (int ix = 0; ix < PNx; ix++)
            {
                for (int iy = 0; iy < PNy; iy++)
                {
                    Agent currentAgent = agents[ix, iy];
                    Agent rightNeighbour = agents[(ix + 1) % PNx, iy];
                    Agent bottomNeighbour = agents[ix, (iy + 1) % PNy];
                    game.Play(currentAgent, rightNeighbour);
                    game.Play(currentAgent, bottomNeighbour);
                }
            }
        }
        
        private void playNoWrapping(int PNx, int PNy)
        {   // Play without wrapping
            for (int ix = 0; ix < PNx; ix++)
            {
                for (int iy = 0; iy < PNy; iy++)
                {
                    Agent currentAgent = agents[ix, iy];
                    if (ix < PNx - 1)
                    {
                        Agent rightNeighbour = agents[ix + 1, iy];
                        game.Play(currentAgent, rightNeighbour);
                    }
                    if (iy < PNy - 1)
                    {
                        Agent bottomNeighbour = agents[ix, iy + 1];
                        game.Play(currentAgent, bottomNeighbour);
                    }
                }
            }
        }

        private void FinaliseRound(int PNx, int PNy)
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
