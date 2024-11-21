/**
 * This file is used to import all the necessary namespaces and classes that are used in the plugin.
 * This file is then imported in ALL the files in the plugin.
 *
 * you never have to worry about importing the same namespaces in every file. Especially usefull für utility classes.
 */
global using static SamplePlugin.Service;
global using static ECommons.GenericHelpers;
global using static SamplePlugin.Util.Utils;
global using static SamplePlugin.Util.TaskManagerUtil;
