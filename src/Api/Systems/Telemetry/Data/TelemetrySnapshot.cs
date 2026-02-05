namespace TgCore.Api.Systems.Telemetry.Data;

public sealed class TelemetrySnapshot
{
    private readonly object _lock = new();
    private readonly TelemetryConfig _config;

    private readonly Queue<TelemetryUpdate> _updates = new();
    private readonly Queue<TelemetryRequest> _requests = new();
    private readonly Queue<TelemetryError> _errors = new();
    private readonly Queue<double> _updateHandlerLatency = new();
    private readonly Queue<double> _errorHandlerLatency = new();

    private readonly HashSet<TelemetryUser> _users = new();
    private readonly HashSet<TelemetryChat> _chats = new();
    private readonly HashSet<long> _uniqueUsersIds = new();
    private readonly HashSet<long> _uniqueChatsIds = new();

    private long _id;

    // -- Updates
    private long _periodUpdatesCount;
    private long _totalUpdatesCount;

    // -- Users
    private long _periodUniqueUsersCount;
    private long _totalUniqueUsersCount;

    // -- Chats
    private long _periodUniqueChatCount;
    private long _totalUniqueChatCount;

    // -- Requests
    private long _periodRequestsCount;
    private long _totalRequestsCount;
    private long _periodSuccessfulRequestsCount;
    private long _periodFailedRequestsCount;
    private long _totalSuccessfulRequestsCount;
    private long _totalFailedRequestsCount;

    // -- Errors
    private long _periodErrorsCount;
    private long _totalErrorsCount;

    internal TelemetrySnapshot(TelemetryConfig config)
    {
        _config = config;
    }

    internal TelemetrySnapshotDto Get()
    {
        lock (_lock)
        {
            _id++;

            return new TelemetrySnapshotDto
            {
                Id = _id,
                Timestamp = DateTime.UtcNow,

                ApiErrors = _errors
                    .Where(e => e.IsTelegram && e.ApiException != null)
                    .GroupBy(e => new
                    {
                        e.ApiException!.ErrorCode,
                        e.ApiException.Method,
                        e.ApiException.Description
                    })
                    .OrderByDescending(g => g.Count())
                    .Select(g => new ApiErrorStats(
                        g.Key.ErrorCode,
                        g.Key.Method!,
                        g.Key.Description!,
                        g.Count()
                    ))
                    .ToList(),

                LocalErrors = _errors
                    .Where(e => e.IsLocal)
                    .GroupBy(e => new
                    {
                        ExceptionType = e.Exception.GetType().Name,
                        Method = e.Exception.TargetSite?.Name,
                        Message = e.Exception.Message
                    })
                    .OrderByDescending(g => g.Count())
                    .Select(g => new LocalErrorStats(
                        g.Key.ExceptionType,
                        g.Key.Method!,
                        g.Key.Message,
                        g.Count()
                    ))
                    .ToList(),


                Users = _users
                    .Select(u => new TelemetryUserDto
                    {
                        Id = u.Id,
                        Name = u.Name,
                        LanguageCode = u.LanguageCode,
                        IsPremium = u.IsPremium,

                        TotalUpdates = u.TotalUpdates,
                        PeriodUpdates = u.PeriodUpdates,

                        FirstActive = u.FirstActive,
                        LastActive = u.LastActive,

                        Interaction = new TelemetryInteractionDto
                        {
                            LastContext = u.Interaction.LastContext.ToList(),
                            MessagesSent = u.Interaction.MessagesSent,
                            CommandsUsed = u.Interaction.CommandsUsed,
                            InlineQueries = u.Interaction.InlineQueries,
                            CallbackQueries = u.Interaction.CallbackQueries,
                            Payments = u.Interaction.Payments,
                            PollAnswers = u.Interaction.PollAnswers,
                            ChatMembershipChanges = u.Interaction.ChatMembershipChanges
                        }
                    })
                    .ToList(),

                Chats = _chats.Select(u => new TelemetryChatDto
                {
                    Id = u.Id,
                    Name = u.Title,

                    Type = u.Type,
                    TotalUpdates = u.TotalUpdates,
                    PeriodUpdates = u.PeriodUpdates,

                    LastActive = u.LastActive,
                    FirstActive = u.FirstActive,

                    Interaction = new TelemetryInteractionDto
                    {
                        LastContext = u.Interaction.LastContext.ToList(),
                        MessagesSent = u.Interaction.MessagesSent,
                        CommandsUsed = u.Interaction.CommandsUsed,
                        InlineQueries = u.Interaction.InlineQueries,
                        CallbackQueries = u.Interaction.CallbackQueries,
                        Payments = u.Interaction.Payments,
                        PollAnswers = u.Interaction.PollAnswers,
                        ChatMembershipChanges = u.Interaction.ChatMembershipChanges
                    }
                }).ToList(),

                Updates = _updates
                    .Select(u => new TelemetryUpdateDto
                    {
                        Id = u.Id,
                        Type = u.Type,
                        CreateDate = u.CreateDate,
                    }).ToList(),

                Requests = _requests
                    .Select(u => new TelemetryRequestDto
                    {
                        Method = u.Method,
                        ErrorMessage = u.ErrorMessage,
                        CreateDate = u.CreateDate,
                        LatencyMs = u.LatencyMs,
                        IsSuccess = u.IsSuccess,
                    }).ToList(),

                Errors = _errors
                    .Select(u => new TelemetryErrorDto
                    {
                        Exception = u.Exception,
                        ApiException = u.ApiException,
                        CreateDate = u.CreateDate,
                    }).ToList(),
                
                UpdateHandlerLatency = new TelemetryLatencyStats(
                    _updateHandlerLatency.Count > 0 ? _updateHandlerLatency.Average() : 0,
                    _updateHandlerLatency.Count > 0 ? _updateHandlerLatency.Min() : 0,
                    _updateHandlerLatency.Count > 0 ? _updateHandlerLatency.Max() : 0),
                
                ErrorHandlerLatency  = new TelemetryLatencyStats(
                    _errorHandlerLatency.Count > 0 ? _errorHandlerLatency.Average() : 0,
                    _errorHandlerLatency.Count > 0 ? _errorHandlerLatency.Min() : 0,
                    _errorHandlerLatency.Count > 0 ? _errorHandlerLatency.Max() : 0),

                HTTPLatency = new TelemetryLatencyStats(
                    _requests.Count > 0 ? _requests.Select(r => r.LatencyMs).Average() : 0,
                    _requests.Count > 0 ? _requests.Select(r => r.LatencyMs).Min() : 0,
                    _requests.Count > 0 ? _requests.Select(r => r.LatencyMs).Max() : 0),

                UpdatesPerUser = _totalUniqueUsersCount > 0 
                    ? (double)_totalUpdatesCount / _totalUniqueUsersCount 
                    : 0,
    
                RequestsPerUser = _totalUniqueUsersCount > 0 
                    ? (double)_totalRequestsCount / _totalUniqueUsersCount 
                    : 0,
    
                UpdatesPerChat = _totalUniqueChatCount > 0 
                    ? (double)_totalUpdatesCount / _totalUniqueChatCount 
                    : 0,
    
                RequestsPerChat = _totalUniqueChatCount > 0 
                    ? (double)_totalRequestsCount / _totalUniqueChatCount 
                    : 0,
                
                TotalSuccessfulRequests = _totalSuccessfulRequestsCount,
                TotalFailedRequests = _totalFailedRequestsCount,
                
                PeriodSuccessfulRequests = _periodSuccessfulRequestsCount,
                PeriodFailedRequests = _periodFailedRequestsCount,

                PeriodUpdates = _periodUpdatesCount,
                TotalUpdates = _totalUpdatesCount,

                PeriodRequests = _periodRequestsCount,
                TotalRequests = _totalRequestsCount,

                PeriodErrors = _periodErrorsCount,
                TotalErrors = _totalErrorsCount,

                PeriodUniqueUsers = _periodUniqueUsersCount,
                TotalUniqueUsers = _totalUniqueUsersCount,

                PeriodUniqueChats = _periodUniqueChatCount,
                TotalUniqueChats = _totalUniqueChatCount,
            };
        }
    }

    internal void AddUpdate(Update? update)
    {
        if (update == null) return;

        lock (_lock)
        {
            var tUpdate = CreateTUpdateFromUpdate(update);
            var tUser = CreateTUserFromUpdate(update);
            var tChat = CreateTChatFromUpdate(update);

            if (_updates.Count >= _config.MaxUpdates)
                _updates.Dequeue();

            _updates.Enqueue(tUpdate);

            if (tUser != null)
            {
                var existingUser = _users.FirstOrDefault(u => u.Id == tUser.Id);

                if (existingUser != null)
                {
                    existingUser.TotalUpdates++;
                    existingUser.PeriodUpdates++;
                    existingUser.LastActive = DateTime.UtcNow;

                    UpdateInteraction(existingUser.Interaction, update);
                }
                else
                {
                    if (_users.Count >= _config.MaxUsers)
                        RemoveLeastUser();

                    if (_uniqueUsersIds.Add(tUser.Id)) 
                    {
                        _periodUniqueUsersCount++;
                        _totalUniqueUsersCount++;
                    }

                    tUser.TotalUpdates++;
                    tUser.PeriodUpdates++;
                    tUser.LastActive = DateTime.UtcNow;

                    UpdateInteraction(tUser.Interaction, update);

                    _users.Add(tUser);
                }
            }

            if (tChat != null && (tChat.Type != ChatType.Private || _config.AllowPrivateChat))
            {
                var existingChat = _chats.FirstOrDefault(c => c.Id == tChat.Id);

                if (existingChat != null)
                {
                    existingChat.TotalUpdates++;
                    existingChat.PeriodUpdates++;
                    existingChat.LastActive = DateTime.UtcNow;

                    UpdateInteraction(existingChat.Interaction, update);
                }
                else
                {
                    if (_chats.Count >= _config.MaxChats)
                        RemoveLeastChat();

                    if (_uniqueChatsIds.Add(tChat.Id)) 
                    {
                        _periodUniqueChatCount++;
                        _totalUniqueChatCount++;
                    }

                    tChat.TotalUpdates++;
                    tChat.PeriodUpdates++;
                    tChat.LastActive = DateTime.UtcNow;

                    UpdateInteraction(tChat.Interaction, update);

                    _chats.Add(tChat);
                }
            }

            _periodUpdatesCount++;
            _totalUpdatesCount++;
        }
    }

    public void AddUpdateHandlerLatency(double latency)
    {
        lock (_lock)
        {
            if (_updateHandlerLatency.Count >= _config.MaxUpdateHandlers)
                _updateHandlerLatency.Dequeue();
            
            _updateHandlerLatency.Enqueue(latency);
        }
    }
    
    public void AddErrorHandlerLatency(double latency)
    {
        lock (_lock)
        {
            if (_errorHandlerLatency.Count >= _config.MaxErrorHandlers)
                _errorHandlerLatency.Dequeue();
            
            _errorHandlerLatency.Enqueue(latency);
        }
    }
    
    public void AddRequest(TelemetryRequest request)
    {
        lock (_lock)
        {
            if (_requests.Count >= _config.MaxRequests)
                _requests.Dequeue();

            _requests.Enqueue(request);

            _periodRequestsCount++;
            _totalRequestsCount++;

            if (request.IsSuccess)
            {
                _totalSuccessfulRequestsCount++;
                _periodSuccessfulRequestsCount++;
            }
            else
            {
                _totalFailedRequestsCount++;
                _periodFailedRequestsCount++;
            }
        }
    }

    public void AddError(TelemetryError error)
    {
        lock (_lock)
        {
            if (_errors.Count >= _config.MaxErrors)
                _errors.Dequeue();

            _errors.Enqueue(error);

            _periodErrorsCount++;
            _totalErrorsCount++;
        }
    }

    public void ClearUniqueCache()
    {
        lock (_lock)
        {
            _uniqueChatsIds.Clear();
            _uniqueUsersIds.Clear();
        }
    }

    internal void Reset()
    {
        lock (_lock)
        {
            _periodUpdatesCount = 0;
            _periodRequestsCount = 0;
            _periodErrorsCount = 0;
            _periodUniqueUsersCount = 0;
            _periodUniqueChatCount = 0;
            _periodFailedRequestsCount = 0;
            _periodSuccessfulRequestsCount = 0;
            
            foreach (var user in _users)
                user.PeriodUpdates = 0;
            
            foreach (var chat in _chats)
                chat.PeriodUpdates = 0;
        }
    }

    private TelemetryUpdate CreateTUpdateFromUpdate(Update update)
    {
        return new TelemetryUpdate(update.Id, update.Type);
    }

    private TelemetryUser? CreateTUserFromUpdate(Update update)
    {
        var user = update.GetFrom;
        if (user == null) return null;

        return new TelemetryUser(
            user.Id,
            user.Username ?? user.FirstName ?? user.LastName,
            user.LanguageCode,
            user.IsPremium,
            _config);
    }

    private TelemetryChat? CreateTChatFromUpdate(Update update)
    {
        var chat = update.GetChat;
        if (chat == null) return null;

        return new TelemetryChat(
            chat.Id,
            chat.Title ?? chat.Username ?? chat.FirstName ?? chat.LastName,
            chat.Type,
            _config);
    }

    private void RemoveLeastUser()
    {
        if (!_users.Any()) return;

        var candidate = _users
            .OrderBy(u => u.PeriodUpdates)
            .ThenBy(u => u.FirstActive)
            .First();

        _users.Remove(candidate);
    }

    private void RemoveLeastChat()
    {
        if (!_chats.Any()) return;

        var candidate = _chats
            .OrderBy(u => u.PeriodUpdates)
            .ThenBy(u => u.FirstActive)
            .First();

        _chats.Remove(candidate);
    }

    private void UpdateInteraction(TelemetryInteraction interaction, Update update)
    {
        switch (update.Type)
        {
            case UpdateType.Message:
                interaction.MessagesSent++;

                if (update.Message?.Text?.StartsWith("/") == true)
                    interaction.CommandsUsed++;
                break;

            case UpdateType.InlineQuery:
                interaction.InlineQueries++;
                break;

            case UpdateType.CallbackQuery:
                interaction.CallbackQueries++;
                break;

            case UpdateType.ShippingQuery:
            case UpdateType.PreCheckoutQuery:
                interaction.Payments++;
                break;

            case UpdateType.ChosenInlineResult:
                interaction.ChosenInlineResults++;
                break;

            case UpdateType.PollAnswer:
            case UpdateType.Poll:
                interaction.PollAnswers++;
                break;

            case UpdateType.MyChatMember:
            case UpdateType.ChatMember:
                interaction.ChatMembershipChanges++;
                break;
        }

        interaction.AddMessage(update.Text);
    }
}

public sealed class TelemetrySnapshotDto
{
    public long Id { get; init; }
    public DateTime Timestamp { get; init; }

    // Counters
    public long PeriodUpdates { get; init; }
    public long TotalUpdates { get; init; }

    public long PeriodRequests { get; init; }
    public long TotalRequests { get; init; }
    
    public long PeriodSuccessfulRequests { get; init; }
    public long PeriodFailedRequests { get; init; }
    public long TotalSuccessfulRequests { get; init; }
    public long TotalFailedRequests { get; init; }

    public long PeriodErrors { get; init; }
    public long TotalErrors { get; init; }

    public long PeriodUniqueUsers { get; init; }
    public long TotalUniqueUsers { get; init; }
    
    public long PeriodUniqueChats { get; init; }
    public long TotalUniqueChats { get; init; }

    // Derived metrics
    public double UpdatesPerUser { get; init; }
    public double RequestsPerUser { get; init; }
    
    public double UpdatesPerChat { get; init; }
    public double RequestsPerChat { get; init; }

    public TelemetryLatencyStats UpdateHandlerLatency { get; init; }
    public TelemetryLatencyStats ErrorHandlerLatency { get; init; }
    public TelemetryLatencyStats HTTPLatency { get; init; }

    // Data
    public List<TelemetryUserDto> Users { get; init; }
    public List<TelemetryChatDto> Chats { get; init; }
    public List<TelemetryUpdateDto> Updates { get; init; }
    public List<TelemetryRequestDto> Requests { get; init; }
    public List<TelemetryErrorDto> Errors { get; init; }

    // Aggregations
    public List<ApiErrorStats> ApiErrors { get; init; }
    public List<LocalErrorStats> LocalErrors { get; init; }
}