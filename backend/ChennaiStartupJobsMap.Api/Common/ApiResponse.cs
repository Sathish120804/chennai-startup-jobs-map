using System;
using System.Collections.Generic;

namespace ChennaiStartupJobsMap.Api.Common
{
    public class ApiResponse<T>
    {
        public bool Success { get; set; } = true;
        public string Message { get; set; } = string.Empty;
        public T? Data { get; set; }
        public List<string> Errors { get; set; } = new();

        public static ApiResponse<T> Ok(T data, string message = "Success") =>
            new() { Success = true, Message = message, Data = data };

        public static ApiResponse<T> Fail(string message, List<string>? errors = null) =>
            new() { Success = false, Message = message, Errors = errors ?? new() };
    }

    public class PagedResponse<T>
    {
        public bool Success { get; set; } = true;
        public string Message { get; set; } = "Success";
        public List<T> Items { get; set; } = new();
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 20;
        public int TotalItems { get; set; }
        public int TotalPages => PageSize > 0 ? (int)Math.Ceiling((double)TotalItems / PageSize) : 0;
        public bool HasNextPage => Page < TotalPages;
        public bool HasPreviousPage => Page > 1;

        public static PagedResponse<T> Create(List<T> items, int totalItems, int page, int pageSize, string message = "Success") =>
            new()
            {
                Success = true,
                Message = message,
                Items = items,
                TotalItems = totalItems,
                Page = page,
                PageSize = pageSize
            };
    }

    public class ErrorResponse
    {
        public bool Success { get; set; } = false;
        public string Message { get; set; } = "An error occurred.";
        public string Code { get; set; } = "INTERNAL_SERVER_ERROR";
        public List<string> Errors { get; set; } = new();
    }
}
