import React from 'react'
import CoinsTable from './CoinsTable';
import DeletePortfolio from '../LoadedPortfolio/DeletePortfolio';

const CoinPortfolio = ({ coins, setFileUploaded, checkIfHasCoins }) => {

  return (
    <>
      <DeletePortfolio
                setFileUploaded={setFileUploaded}
                checkIfHasCoins={checkIfHasCoins}
            />
      <CoinsTable coins={coins} tableCaption={'Your Positions'}/>
    </>
  )
}

export default CoinPortfolio