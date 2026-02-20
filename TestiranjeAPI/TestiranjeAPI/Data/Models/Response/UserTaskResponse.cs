namespace TestiranjeAPI.Models.Response;

public record TaskDataResponse(int TaskId, string TaskName, string TaskDescription);
public record UserTaskResponse(int PartyId, string PartyName, IEnumerable<TaskDataResponse> Tasks);
