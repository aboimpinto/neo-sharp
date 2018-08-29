using System;
using NeoSharp.Core.Blockchain;
using NeoSharp.Core.Extensions;
using NeoSharp.Core.Types;

namespace NeoSharp.Core.Models.Builders
{
    public class BlockBuilder
    {
        //public SignedBlock BuildSignedGenesisBlock()
        //{
        //    //var transactionBuilder = new TransactionBuilder();

        //    //var governingToken = transactionBuilder.BuildSignedGoverningTokenRegisterTransaction();
        //    //var utilityToken = transactionBuilder.BuildGenesisUtilityTokenRegisterTransaction();

        //    //var genesisMinerTransaction = transactionBuilder.BuildUnsignedGenesisMinerTransaction();
        //    //var genesisIssueTransaction = transactionBuilder.BuildUnsignedGenesisIssueTransaction(governingToken.Hash, governingToken.Amount);

        //    //var genesisWitness = new WitnessBuilder().BuildUnsignedGenesisWitness(); 
        //    //var genesisTimestamp = new DateTime(2016, 7, 15, 15, 8, 21, DateTimeKind.Utc).ToTimestamp();
        //    //const ulong genesisConsensusData = 2083236893; //向比特币致敬

        //    //var nextConsensusAddress = GenesisAssets.GetGenesisNextConsensusAddress();

        //    //var unsignedGenesisBlock = new UnsignedBlock
        //    //{
        //    //    PreviousBlockHash = UInt256.Zero,
        //    //    Timestamp = genesisTimestamp,
        //    //    Index = 0,
        //    //    ConsensusData = genesisConsensusData,
        //    //    NextConsensus = nextConsensusAddress,
        //    //    Witness = genesisWitness,
        //    //    Transactions = new TransactionBase[]
        //    //    {
        //    //        genesisMinerTransaction,        //First transaction is always a miner transaction
        //    //        governingToken,                 //Creates NEO 
        //    //        utilityToken,                   //Creates GAS
        //    //        genesisIssueTransaction         //Send all NEO to seed contract
        //    //    }
        //    //};

        //    //return new SignedBlock(unsignedGenesisBlock);

        //    throw new NotImplementedException();
        //}
    }
}
