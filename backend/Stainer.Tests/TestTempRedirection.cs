// 已废弃：早期用 [ModuleInitializer] 重定向 TMP/TEMP 来规避 SQLite 临时库写满系统盘的方案，
// 在测试宿主中运行时机不可靠（验收实测重定向未生效）。改为统一让所有测试通过 TestPaths.TempRoot
// 创建临时库（见 TestPaths.cs）——确定性，不依赖环境变量或模块初始化器。
