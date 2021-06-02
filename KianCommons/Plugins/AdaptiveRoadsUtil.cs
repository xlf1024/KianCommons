namespace KianCommons.Plugins {
    using System;
    using System.Reflection;
    using static ColossalFramework.Plugins.PluginManager;
    using static KianCommons.Plugins.PluginUtil;
    using ColossalFramework.Plugins;

    internal static class AdaptiveRoadsUtil {
        static AdaptiveRoadsUtil() {
            PluginManager.instance.eventPluginsStateChanged +=
                () => {
                    plugin_ = null;
                    nodeLaneTypes_ = null;
                    nodeVehicleTypes_ = null;
                };

        }

        static PluginInfo plugin_;
        static PluginInfo plugin => plugin_ ??= GetAdaptiveRoads();
        public static bool IsActive => plugin.IsActive();

        public static Assembly asm => plugin.GetMainAssembly();
        public static Type API_ => asm.GetType("AdaptiveRoads.API", throwOnError: true, ignoreCase: true);
        static MethodInfo GetMethod(string name) =>
            API_.GetMethod(name) ?? throw new Exception(name + " not found");
        static object Invoke(string methodName, params object[] args) =>
            GetMethod(methodName).Invoke(null, args);

        static TDelegate CreateDelegate<TDelegate>() where TDelegate : Delegate => DelegateUtil.CreateDelegate<TDelegate>(API_);

#pragma warning disable HAA0601, HAA0101
        public static bool IsAdaptive(this NetInfo info) {
            if (!IsActive) return false;
            return (bool)Invoke("IsAdaptive", info);
        }
        public static object GetARSegmentFlags(ushort id) {
            if (!IsActive) return null;
            return Invoke("GetARSegmentFlags", id);
        }
        public static object GetARNodeFlags(ushort id) {
            if (!IsActive) return null;
            return Invoke("GetARNodeFlags", id);
        }
        public static object GetARSegmentEndFlags(ushort segmentID, ushort nodeID) {
            if (!IsActive) return null;
            return Invoke("GetARSegmentEndFlags", segmentID, nodeID);
        }
        public static object GetARSegmentEndFlags(ushort segmentID, bool startNode) {
            if (!IsActive) return null;
            ushort nodeID = segmentID.ToSegment().GetNode(startNode);
            return Invoke("GetARSegmentEndFlags", segmentID, nodeID);
        }
        public static object GetARLaneFlags(uint laneId) {
            if (!IsActive) return null;
            return Invoke("GetARLaneFlags", laneId);
        }

        public delegate VehicleInfo.VehicleType NodeVehicleTypes(NetInfo.Node node);
        static NodeVehicleTypes nodeVehicleTypes_;
        public static VehicleInfo.VehicleType VehicleTypes(this NetInfo.Node node) {
            nodeVehicleTypes_ ??= CreateDelegate<NodeVehicleTypes>();
            return nodeVehicleTypes_(node);
        }


        public delegate NetInfo.LaneType NodeLaneTypes(NetInfo.Node node);
        static NodeLaneTypes nodeLaneTypes_;
        public static NetInfo.LaneType LaneTypes(this NetInfo.Node node) {
            nodeLaneTypes_ ??= CreateDelegate<NodeLaneTypes>();
            return nodeLaneTypes_(node);
        }
#pragma warning restore HAA0101, HAA0601
    }
}