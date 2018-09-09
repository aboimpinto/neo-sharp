using System.Collections.Generic;
using NeoSharp.Core.Blockchain;
using NeoSharp.Core.Blockchain.Processing;
using NeoSharp.Core.Blockchain.State;
using NeoSharp.Core.Models;
using NeoSharp.Core.Models.Blocks;
using NeoSharp.Core.Models.Transactions;
using NeoSharp.Core.Models.Witnesses;
using IssueTransaction = NeoSharp.Core.Models.IssueTransaction;
using RegisterTransaction = NeoSharp.Core.Models.RegisterTransaction;

namespace NeoSharp.Core.DI.Modules
{
    public class BlockchainModule : IModule
    {
        public void Register(IContainerBuilder containerBuilder)
        {
            containerBuilder.RegisterSingleton<IBlockchain, Blockchain.Blockchain>();
            containerBuilder.RegisterSingleton<ICoinIndex, CoinIndex>();

            #region Processing

            containerBuilder.RegisterSingleton<IBlockHeaderPersister, BlockHeaderPersister>();
            containerBuilder.RegisterSingleton<IBlockProcessor, BlockProcessor>();
            containerBuilder.RegisterSingleton<IBlockPool, BlockPool>();
            containerBuilder.RegisterSingleton<IComparer<Transaction>, TransactionComparer>();
            containerBuilder.RegisterSingleton<ITransactionPool, TransactionPool>();
            containerBuilder.RegisterSingleton<ITransactionPersister<Transaction>, TransactionPersister>();

            containerBuilder.RegisterSingleton<ITransactionPersister<ClaimTransaction>, ClaimTransactionPersister>();
            containerBuilder.RegisterSingleton<ITransactionPersister<InvocationTransaction>, InvocationTransactionPersister>();
            containerBuilder.RegisterSingleton<ITransactionPersister<IssueTransaction>, IssueTransactionPersister>();
            containerBuilder.RegisterSingleton<ITransactionPersister<PublishTransaction>, PublishTransactionPersister>();
            containerBuilder.RegisterSingleton<ITransactionPersister<RegisterTransaction>, RegisterTransactionPersister>();
            containerBuilder.RegisterSingleton<ITransactionPersister<StateTransaction>, StateTransactionPersister>();
            containerBuilder.RegisterSingleton<ITransactionPersister<EnrollmentTransaction>, EnrollmentTransactionPersister>();

            #endregion

            containerBuilder.RegisterSingleton<IAccountManager, AccountManager>();

            containerBuilder.RegisterSingleton<IBlockSignatureManager, BlockSignatureManager>();
            containerBuilder.RegisterSingleton<ITransactionSignatureManager, TransactionSignatureManager>();
            containerBuilder.RegisterSingleton<IWitnessSignatureManager, WitnessSignatureManager>();
        }
    }
}