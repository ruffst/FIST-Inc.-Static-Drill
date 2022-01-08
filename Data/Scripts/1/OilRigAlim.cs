using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Sandbox.Common;
using Sandbox.Common.ObjectBuilders;
using Sandbox.Game;
using Sandbox.Game.GameSystems.Conveyors;
using Sandbox.Game.Entities;
using Sandbox.Game.EntityComponents;
using Sandbox.Definitions;
using Sandbox.ModAPI;
using Sandbox.ModAPI.Ingame;
using Sandbox.ModAPI.Interfaces;

using VRage;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.ModAPI;
using VRage.Game.ObjectBuilders.Definitions;
using VRage.ModAPI;
using VRage.ObjectBuilders;
using VRage.Utils;
using VRageMath;
using VRage.Game.Entity;
using VRage.Voxels;
using System.Diagnostics;

namespace P3DResourceRig
{
    [MyEntityComponentDescriptor(typeof(MyObjectBuilder_Assembler), true, "LargeBlockRig1","LargeBlockRig2")]
    public class Rig : MyGameLogicComponent
    {
        // Builder is nessassary for GetObjectBuilder method as far as I know.
        private MyObjectBuilder_EntityBase builder;
        private Sandbox.ModAPI.IMyAssembler m_generator;
        private IMyCubeBlock m_parent;
        #region IsInVoxel definition
        private bool IsInVoxel(Sandbox.ModAPI.IMyTerminalBlock block)
        {
            BoundingBoxD blockWorldAABB = block.PositionComp.WorldAABB;
            List<MyVoxelBase> voxelList = new List<MyVoxelBase>();
            MyGamePruningStructure.GetAllVoxelMapsInBox(ref blockWorldAABB, voxelList);
            var cubeSize = block.CubeGrid.GridSize;
            BoundingBoxD localAAABB = new BoundingBoxD(cubeSize * ((Vector3D)block.Min - 1), cubeSize * ((Vector3D)block.Max + 1));
            var gridWorldMatrix = block.CubeGrid.WorldMatrix;
            foreach (var map in voxelList)
            {
                if (map.IsAnyAabbCornerInside(ref gridWorldMatrix, localAAABB))
                {
                    return true;
                }
            }

            return false;
        }
        #endregion
        #region colors
        private Color m_primaryColor = Color.OrangeRed;
        private Color m_secondaryColor = Color.LemonChiffon;
        public Color PrimaryBeamColor
        {
            get { return m_primaryColor; }
            set
            {
                m_primaryColor = value;               
            }
        }

        public Color SecondaryBeamColor
        {
            get { return m_secondaryColor; }
            set
            {
                m_secondaryColor = value;        
            }
        }
        #endregion
        Sandbox.ModAPI.IMyTerminalBlock terminalBlock;

        public override void Init(MyObjectBuilder_EntityBase objectBuilder)
        {
            m_generator = Entity as Sandbox.ModAPI.IMyAssembler;
            m_parent = Entity as IMyCubeBlock;
            builder = objectBuilder;

            Entity.NeedsUpdate |= MyEntityUpdateEnum.EACH_FRAME | MyEntityUpdateEnum.EACH_100TH_FRAME;

            terminalBlock = Entity as Sandbox.ModAPI.IMyTerminalBlock;
        }
        #region UpdateBeforeSimulation
        public override void UpdateBeforeSimulation100()
        {
            base.UpdateBeforeSimulation100();
            
            if (m_generator.IsWorking)
            {
                if (IsInVoxel(m_generator as Sandbox.ModAPI.IMyTerminalBlock))
                {
                    IMyInventory inventory = ((Sandbox.ModAPI.IMyTerminalBlock)Entity).GetInventory(0) as IMyInventory;
                    VRage.MyFixedPoint amount = (VRage.MyFixedPoint)(70 * (1 + (0.4 * m_generator.UpgradeValues["Productivity"])));
                    inventory.AddItems(amount, new MyObjectBuilder_Ore() { SubtypeName = "Stone" });
                    terminalBlock.RefreshCustomInfo();
                }
            }
        }
        #endregion
        public override MyObjectBuilder_EntityBase GetObjectBuilder(bool copy = false)
        {
            return builder;
        }

        private ulong m_counter = 0;

        //draw counter
        public override void UpdateBeforeSimulation()
        {
            m_counter++;
            if (m_generator.IsWorking)
            {
                if (IsInVoxel(m_generator as Sandbox.ModAPI.IMyTerminalBlock))
                {
                    if (MyAPIGateway.Session?.Player == null)
					{
						return;
					}
					else
					{
						DrawBeams();
					}				
                }
            }
        }

    }
    [MyEntityComponentDescriptor(typeof(MyObjectBuilder_Assembler), true, "LargeBlockRig2")]
    [DebuggerDisplay($"{{{nameof(GetDebuggerDisplay)}(),nq}}")]
    public class Rig3 : MyGameLogicComponent
    {
        // Builder is nessassary for GetObjectBuilder method as far as I know.
        private MyObjectBuilder_EntityBase builder;
        private Sandbox.ModAPI.IMyAssembler m_generator;
        private IMyCubeBlock m_parent;
        #region IsInVoxel definition
        private bool IsInVoxel(Sandbox.ModAPI.IMyTerminalBlock block)
        {
            BoundingBoxD blockWorldAABB = block.PositionComp.WorldAABB;
            List<MyVoxelBase> voxelList = new List<MyVoxelBase>();
            MyGamePruningStructure.GetAllVoxelMapsInBox(ref blockWorldAABB, voxelList);
            var cubeSize = block.CubeGrid.GridSize;
            BoundingBoxD localAAABB = new BoundingBoxD(cubeSize * ((Vector3D)block.Min - 1), cubeSize * ((Vector3D)block.Max + 1));
            var gridWorldMatrix = block.CubeGrid.WorldMatrix;
            foreach (var map in voxelList)
            {
                if (map.IsAnyAabbCornerInside(ref gridWorldMatrix, localAAABB))
                {
                    return true;
                }
            }

            return false;
        }
        #endregion
        #region colors
        private Color m_primaryColor = Color.OrangeRed;
        private Color m_secondaryColor = Color.LemonChiffon;
        public Color PrimaryBeamColor
        {
            get { return m_primaryColor; }
            set
            {
                m_primaryColor = value;               
            }
        }

        public Color SecondaryBeamColor
        {
            get { return m_secondaryColor; }
            set
            {
                m_secondaryColor = value;        
            }
        }
        #endregion
        Sandbox.ModAPI.IMyTerminalBlock terminalBlock;

        public override void Init(MyObjectBuilder_EntityBase objectBuilder)
        {
            m_generator = Entity as Sandbox.ModAPI.IMyAssembler;
            m_parent = Entity as IMyCubeBlock;
            builder = objectBuilder;

            Entity.NeedsUpdate |= MyEntityUpdateEnum.EACH_FRAME | MyEntityUpdateEnum.EACH_100TH_FRAME;

            terminalBlock = Entity as Sandbox.ModAPI.IMyTerminalBlock;
        }
        #region UpdateBeforeSimulation
        public override void UpdateBeforeSimulation100()
        {
            base.UpdateBeforeSimulation100();
            
            if (m_generator.IsWorking)
            {
                if (IsInVoxel(m_generator as Sandbox.ModAPI.IMyTerminalBlock))
                {
                    IMyInventory inventory = ((Sandbox.ModAPI.IMyTerminalBlock)Entity).GetInventory(0) as IMyInventory;
                    VRage.MyFixedPoint amount = (VRage.MyFixedPoint)(140 * (1 + (0.6 * m_generator.UpgradeValues["Productivity"])));
                    inventory.AddItems(amount, new MyObjectBuilder_Ore() { SubtypeName = "Stone" });
                    terminalBlock.RefreshCustomInfo();
                }
            }
        }
        #endregion
        public override MyObjectBuilder_EntityBase GetObjectBuilder(bool copy = false)
        {
            return builder;
        }

        private ulong m_counter = 0;

        //draw counter
        public override void UpdateBeforeSimulation()
        {
            m_counter++;
            if (m_generator.IsWorking)
            {
                if (IsInVoxel(m_generator as Sandbox.ModAPI.IMyTerminalBlock))
                {
                    if (MyAPIGateway.Session?.Player == null)
					{
						return;
					}
					else
					{
						DrawBeams();
					}				
                }
            }
        }

    [MyEntityComponentDescriptor(typeof(MyObjectBuilder_Assembler), true, "LargeBlockRig3")]
    public class Rig3 : MyGameLogicComponent
    {
        // Builder is nessassary for GetObjectBuilder method as far as I know.
        private MyObjectBuilder_EntityBase builder;
        private Sandbox.ModAPI.IMyAssembler m_generator;
        private IMyCubeBlock m_parent;
        #region IsInVoxel definition
        private bool IsInVoxel(Sandbox.ModAPI.IMyTerminalBlock block)
        {
            BoundingBoxD blockWorldAABB = block.PositionComp.WorldAABB;
            List<MyVoxelBase> voxelList = new List<MyVoxelBase>();
            MyGamePruningStructure.GetAllVoxelMapsInBox(ref blockWorldAABB, voxelList);
            var cubeSize = block.CubeGrid.GridSize;
            BoundingBoxD localAAABB = new BoundingBoxD(cubeSize * ((Vector3D)block.Min - 1), cubeSize * ((Vector3D)block.Max + 1));
            var gridWorldMatrix = block.CubeGrid.WorldMatrix;
            foreach (var map in voxelList)
            {
                if (map.IsAnyAabbCornerInside(ref gridWorldMatrix, localAAABB))
                {
                    return true;
                }
            }

            return false;
        }
        #endregion
        #region colors
        private Color m_primaryColor = Color.OrangeRed;
        private Color m_secondaryColor = Color.LemonChiffon;
        public Color PrimaryBeamColor
        {
            get { return m_primaryColor; }
            set
            {
                m_primaryColor = value;               
            }
        }

        public Color SecondaryBeamColor
        {
            get { return m_secondaryColor; }
            set
            {
                m_secondaryColor = value;        
            }
        }
        #endregion
        Sandbox.ModAPI.IMyTerminalBlock terminalBlock;

        public override void Init(MyObjectBuilder_EntityBase objectBuilder)
        {
            m_generator = Entity as Sandbox.ModAPI.IMyAssembler;
            m_parent = Entity as IMyCubeBlock;
            builder = objectBuilder;

            Entity.NeedsUpdate |= MyEntityUpdateEnum.EACH_FRAME | MyEntityUpdateEnum.EACH_100TH_FRAME;

            terminalBlock = Entity as Sandbox.ModAPI.IMyTerminalBlock;
        }
        #region UpdateBeforeSimulation
        public override void UpdateBeforeSimulation100()
        {
            base.UpdateBeforeSimulation100();
            
            if (m_generator.IsWorking)
            {
                if (IsInVoxel(m_generator as Sandbox.ModAPI.IMyTerminalBlock))
                {
                    IMyInventory inventory = ((Sandbox.ModAPI.IMyTerminalBlock)Entity).GetInventory(0) as IMyInventory;
                    VRage.MyFixedPoint amount = (VRage.MyFixedPoint)(300 * (1 + (0.8 * m_generator.UpgradeValues["Productivity"])));
                    inventory.AddItems(amount, new MyObjectBuilder_Ore() { SubtypeName = "Stone" });
                    terminalBlock.RefreshCustomInfo();
                }
            }
        }
        #endregion
        public override MyObjectBuilder_EntityBase GetObjectBuilder(bool copy = false)
        {
            return builder;
        }

        private ulong m_counter = 0;

        //draw counter
        public override void UpdateBeforeSimulation()
        {
            m_counter++;
            if (m_generator.IsWorking)
            {
                if (IsInVoxel(m_generator as Sandbox.ModAPI.IMyTerminalBlock))
                {
                    if (MyAPIGateway.Session?.Player == null)
					{
						return;
					}
					else
					{
						DrawBeams();
					}				
                }
            }
        }




        private void DrawBeams()
        {
            var maincolor = PrimaryBeamColor.ToVector4();
            var auxcolor = SecondaryBeamColor.ToVector4();

            VRage.Game.MySimpleObjectDraw.DrawLine(m_generator.WorldAABB.Center - (m_generator.WorldMatrix.Down * 2.5), m_generator.WorldAABB.Center + (m_generator.WorldMatrix.Down * 2.5 * 4), VRage.Utils.MyStringId.GetOrCompute("WeaponLaser"), ref auxcolor, 0.33f);
            VRage.Game.MySimpleObjectDraw.DrawLine(m_generator.WorldAABB.Center - (m_generator.WorldMatrix.Down * 2.5), m_generator.WorldAABB.Center + (m_generator.WorldMatrix.Down * 2.5 * 4), VRage.Utils.MyStringId.GetOrCompute("WeaponLaser"), ref maincolor, 1.02f);

            // Draw 'pulsing' beam
            if (m_counter % 2 == 0)
            {
                VRage.Game.MySimpleObjectDraw.DrawLine(m_generator.WorldAABB.Center - (m_generator.WorldMatrix.Down * 2.5), m_generator.WorldAABB.Center + (m_generator.WorldMatrix.Down * 2.5 * 4), VRage.Utils.MyStringId.GetOrCompute("WeaponLaser"), ref maincolor, 1.12f);
            }        
        }
    }
}