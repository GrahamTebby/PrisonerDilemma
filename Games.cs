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
        public event EventHandler<FrameCountEventArgs>? RoundPlayed;
        readonly private Agent[,] agents;
        readonly private CheckBox torroidalField;
        readonly FrameCountEventArgs frameCountEventArgs;
        private Game game;
        private TextBox? frameCountTbox;
        private int nx, ny;

        public Games(Agent[,] PAgents, CheckBox PTorroidalFieldCbox, Game PGame)
        {
            agents = PAgents;
            game = PGame;
            torroidalField = PTorroidalFieldCbox;
            frameCountEventArgs = new FrameCountEventArgs(0);
            nx = agents.GetLength(0);
            ny = agents.GetLength(1);
        }

        internal void PlayRound(object PSender, EventArgs PE)
        {
            frameCountEventArgs.FrameCount += 1;
            initAgents(); // Initialise the agents for the round

            if (torroidalField.Checked)
            {   
                playTorroidal(nx, ny);  // Play with toroidal wrapping
            }
            else
            {   
                playNoWrapping(nx, ny); // Play without wrapping
            }
            
            finaliseRound(nx, ny);      // Finalise the round for all agents

            RoundPlayed?.Invoke(this, frameCountEventArgs);
        }


        private void initAgents()
        {   // Initialise all agents for the round
            for (int ix = 0; ix < nx; ix++)
            {
                for (int iy = 0; iy < ny; iy++)
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

    public class FrameCountEventArgs : EventArgs
    {
        public int FrameCount { get; set; }
        public FrameCountEventArgs(int PFrameCount)
        {
            FrameCount = PFrameCount;
        }
    }
}
