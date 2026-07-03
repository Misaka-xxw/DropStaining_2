namespace Stainer.Web.Application.Services;

public static class OperatorAlarmPresentation
{
    public static string Category(string? code)
    {
        var value = code ?? string.Empty;
        if (value.Contains("database", StringComparison.OrdinalIgnoreCase)) return "数据库维护";
        if (value.Contains("reagent", StringComparison.OrdinalIgnoreCase)) return "试剂状态";
        if (value.Contains("dab", StringComparison.OrdinalIgnoreCase)) return "DAB 状态";
        if (value.Contains("thermal", StringComparison.OrdinalIgnoreCase)
            || value.Contains("cooling", StringComparison.OrdinalIgnoreCase)) return "温控状态";
        if (value.Contains("fluid", StringComparison.OrdinalIgnoreCase)
            || value.Contains("pump", StringComparison.OrdinalIgnoreCase)
            || value.Contains("liquid", StringComparison.OrdinalIgnoreCase)
            || value.Contains("wash", StringComparison.OrdinalIgnoreCase)) return "液路状态";
        if (value.Contains("startup", StringComparison.OrdinalIgnoreCase)) return "启动恢复";
        if (value.Contains("device", StringComparison.OrdinalIgnoreCase)
            || value.Contains("motion", StringComparison.OrdinalIgnoreCase)
            || value.Contains("command", StringComparison.OrdinalIgnoreCase)
            || value.Contains("communication", StringComparison.OrdinalIgnoreCase)
            || value.Contains("mock_fault", StringComparison.OrdinalIgnoreCase)) return "设备状态";
        return "系统提示";
    }

    public static string Summary(string? code, string? severity)
    {
        var value = code ?? string.Empty;
        if (value.Equals("database_backup_degraded", StringComparison.OrdinalIgnoreCase))
        {
            return "数据库备份已完成，临时文件清理待工程维护。";
        }

        if (value.Contains("database_backup", StringComparison.OrdinalIgnoreCase))
        {
            return "数据库维护未完成，请联系工程人员处理。";
        }

        return Category(value) switch
        {
            "试剂状态" => "试剂状态需要处理，请检查试剂后按界面指引重试。",
            "DAB 状态" => "DAB 状态需要处理，请按界面指引检查。",
            "温控状态" => "温控状态异常，请暂停相关操作并联系工程人员。",
            "液路状态" => "液路状态异常，请检查耗材并联系工程人员。",
            "启动恢复" => "系统恢复尚未完成，请联系工程人员。",
            "设备状态" => "设备状态异常，请暂停相关操作并联系工程人员。",
            _ when severity is "Critical" or "Error" => "系统运行异常，请暂停相关操作并联系工程人员。",
            _ => "系统提示需要处理，请按界面指引检查。"
        };
    }

    public static string ActionSummary(string? action) => action switch
    {
        "Acknowledged" => "告警已确认。",
        "Resolved" => "告警已处理。",
        _ => "处理状态已记录。"
    };

    public static (string Title, string Detail) AuditSummary(string? action)
    {
        var value = action ?? string.Empty;
        if (value.Contains("database.backup", StringComparison.OrdinalIgnoreCase))
        {
            return ("数据库维护", "数据库维护操作已记录；技术详情请由工程人员查看。");
        }

        if (value.Contains("device", StringComparison.OrdinalIgnoreCase)
            || value.Contains("engineering", StringComparison.OrdinalIgnoreCase))
        {
            return ("设备维护", "设备维护操作已记录。");
        }

        if (value.Contains("workflow", StringComparison.OrdinalIgnoreCase)) return ("流程配置", "流程配置变更已记录。");
        if (value.Contains("reagent", StringComparison.OrdinalIgnoreCase)) return ("试剂操作", "试剂操作已记录。");
        if (value.Contains("auth.", StringComparison.OrdinalIgnoreCase)) return ("用户操作", "用户会话操作已记录。");
        return ("系统操作", "系统操作已记录。");
    }
}
