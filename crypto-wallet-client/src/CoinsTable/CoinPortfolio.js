import React from 'react'
import CoinsTable from './CoinsTable';
import DeletePortfolio from '../LoadedPortfolio/DeletePortfolio';

const CoinPortfolio = ({ coins, setFileUploaded, checkIfHasCoins }) => {
  const hasApiCalculations = true;

  return (
    <>
      <DeletePortfolio
                setFileUploaded={setFileUploaded}
                checkIfHasCoins={checkIfHasCoins}
            />
      <CoinsTable coins={coins} tableCaption={'Your Positions'} hasApiCalculations={hasApiCalculations} />
    </>
  )
}

export default CoinPortfolio