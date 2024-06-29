using System;
using GTA;
using GTA.Math;
using GTA.Native;
using GTA.UI;
using System.Windows.Forms;

public class AutoRunAvoidObstacles : Script
{
    private bool autoRunEnabled = false;
    private readonly float runSpeed = 5.0f; // Make it readonly
    private readonly float obstacleAvoidanceDistance = 2.0f; // Make it readonly

    public AutoRunAvoidObstacles()
    {
        Tick += OnTick;
        KeyDown += OnKeyDown;
    }

    private void OnTick(object sender, EventArgs e)
    {
        if (autoRunEnabled)
        {
            Ped player = Game.Player.Character;
            Vector3 currentPosition = player.Position;
            Vector3 forwardVector = player.ForwardVector;
            Vector3 newPosition = currentPosition + forwardVector * runSpeed * Game.LastFrameTime;

            // Check for obstacles ahead
            if (IsObstacleAhead(currentPosition, forwardVector))
            {
                // Adjust direction to avoid obstacle (e.g., turn left or right)
                Vector3 avoidanceDirection = Vector3.Cross(forwardVector, Vector3.WorldUp).Normalized;
                newPosition += avoidanceDirection * runSpeed * Game.LastFrameTime;
            }

            // Move the player
            player.Task.RunTo(newPosition);
        }
    }

    private bool IsObstacleAhead(Vector3 currentPosition, Vector3 forwardVector)
    {
        // Example: Check if there's an obstacle within a certain distance ahead
        RaycastResult raycast = World.Raycast(currentPosition, forwardVector, obstacleAvoidanceDistance, GTA.IntersectOptions.Everything, Game.Player.Character);
        return raycast.DidHit; // Use DidHit instead of DidHitAnything
    }

    private void OnKeyDown(object sender, KeyEventArgs e)
    {
        if (e.KeyCode == Keys.F6) // Compare, don't assign
        {
            autoRunEnabled = !autoRunEnabled;
            Notification.Show("Auto Run: " + (autoRunEnabled ? "Enabled" : "Disabled"));
        }
    }
}
