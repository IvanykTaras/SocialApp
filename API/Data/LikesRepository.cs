using System;
using API.Entities;
using API.Extensions;
using API.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace API.Data;

public class LikesRepository : ILikesRepository
{
    private readonly AppDbContext _context;
    public LikesRepository(AppDbContext context)
    {
        _context = context;
    }
    public void AddLike(MemberLike like)
    {
        _context.Likes.Add(like);
    }

    public void DeleteLike(MemberLike like)
    {
        _context.Likes.Remove(like);
    }

    public async Task<IReadOnlyList<string>> GetCurrentMemberLikeIds(string memberId)
    {
        return await _context.Likes
            .Where(x => x.SourceMemberId == memberId)
            .Select(x => x.TargetMemberId)
            .ToListAsync();
    }

    public async Task<MemberLike?> GetMemberLike(string sourceMemberId, string targetMemberId)
    {
        return await _context.Likes.FindAsync(sourceMemberId, targetMemberId);
    }
    

    public async Task<IReadOnlyList<Member>> GetMemberLikes(string predicate, string memberId)
    {
        var query = _context.Likes.AsQueryable();

        switch (predicate)
        {
            case "liked":
                return await query
                    .Where(x => x.SourceMemberId == memberId)
                    .Select(x => x.TargetMember)
                    .ToListAsync();
                
            case "likedBy":
                return await query
                    .Where(x => x.TargetMemberId == memberId)
                    .Select(x => x.SourceMember)
                    .ToListAsync();
            default:
                var likeIds = await GetCurrentMemberLikeIds(memberId);
                return await query
                    .Where(x => x.TargetMemberId == memberId 
                        && likeIds.Contains(x.SourceMemberId))
                    .Select(x => x.SourceMember)
                    .ToListAsync();
        }
    }

    public async Task<bool> SaveAllChanges()
    {
        return await _context.SaveChangesAsync() > 0;
    }
}

internal class DataContext
{
}