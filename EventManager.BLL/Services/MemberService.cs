using EventManager.BLL.Interfaces;
using EventManager.DAL.Interfaces;
using EventManager.Domain.Constants;
using EventManager.Domain.Entities;
using Isopoh.Cryptography.Argon2;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace EventManager.BLL.Services
{
    public class MemberService : IMemberService
    {
        private readonly IMemberRepository memberRepository;

        public MemberService(IMemberRepository memberRepository)
        {
            this.memberRepository = memberRepository;
        }


        public bool CheckAvailableEmail(string email)
        {
            int indexAt = email.IndexOf("@");
            if(indexAt < 0 || indexAt > email.Length -1 || indexAt != email.LastIndexOf("@"))
            {
                return false;
            }

            if (EmailConstant.DisposableDomain
                    .Select(domain => $@"^.*@{domain}$")
                    .Any(domain => Regex.IsMatch(email, domain, RegexOptions.IgnoreCase)))
            {
                return false;
            }

            return !memberRepository.EmailExists(email);
        }

        public bool CheckAvailablePseudo(string pseudo)
        {
            if (PseudoConstant.Forbidden.Any(pf => pf.Equals(pseudo, StringComparison.OrdinalIgnoreCase)))
            {
                return false;
            }

            return !memberRepository.PseudoExists(pseudo);
        }

        public Member? GetMember(int id)
        {
            return memberRepository.GetById(id);
        }

        public Member? Login(string identifier, string pwd)
        {
            Member? member = memberRepository.GetByIdentifier(identifier);
            if (member is null)
            {
                return null;
            }

            string hashPwd = memberRepository.GetHashPwd(member.Id) ?? string.Empty;
            if (!Argon2.Verify(hashPwd, pwd))
            {
                return null;
            }

            return member;
        }

        public Member? Register(Member member)
        {
            if (member.Email is null || member.Pseudo is null || member.HashPwd is null 
                || !CheckAvailableEmail(member.Email) || !CheckAvailablePseudo(member.Pseudo))
            {
                return null;
            }

            // Hash du mot de passe de l'utilisateur
            member.HashPwd = Argon2.Hash(member.HashPwd);

            int memberId = memberRepository.Insert(member);
            return memberRepository.GetById(memberId);
        }

        public void UpdateMember(Member member)
        {
            if(memberRepository.GetById(member.Id) is null)
            {
                throw new Exception("Member not exists");
            }

            memberRepository.Update(member);
        }
    }
}
