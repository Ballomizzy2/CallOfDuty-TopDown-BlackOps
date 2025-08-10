using UnityEngine;

public class Notes_TrioMapScene : MonoBehaviour
{
/*
 *          ===ZombieSpawners===
 * there are currently two spawners: Spawner[letter] and SpawnHolderMC
 *      Spawner[Letter] is the orignal spawner set up by Andy and must be enabled for
 * SpawnManager IsUsingOriginalUse to work properly
 *      SpawnerHolderMC is supposed to be a "cleaner" spawner with a visual (minecraft reference)
 * currently doesn't work.
 * It's also really ugly bc it is nested and the actualy spawner is like 2 layers in... (SpawnerHolderMc > VisualHolder > Spawner).
 * 
 *  Problem: the OG spawner kinda spits X num of zombies at every spawner
 *  What we want: the program to randomly populate the spawners based on the current round (i think i have the
 * math set up, but not the actual spawning to test it..)
 * 
 * TODO: have the spawner get data from the spawnmanager for controller spawning?
 * 
 * ===in SpawnManager===
 *   there are multiple spawners and 1 manager, so the manager should hav a reference to each one of the
 * spawners then pop zombies into them. 
 *   SO by using the room data script, the SpawnManager should grab the avalible spawners from that script
 *   to populate the zombies?
 *   
 *   ===og spawner===
 *   every spawner is isolated so will call the number of spawns based on the local spawner script (SpawnInBox)
 *   
 *
 */
}
