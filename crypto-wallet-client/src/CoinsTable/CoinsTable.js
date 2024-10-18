import React, { useState } from 'react'
import CoinRow from './CoinRow'
import DeletePortfolio from '../LoadedPortfolio/DeletePortfolio';

const CoinsTable = ({ coins, tableCaption, setFileUploaded, checkIfHasCoins }) => {
    const [currentPage, setCurrentPage] = useState(1);
    const itemsPerPage = 6;

    const totalPages = Math.ceil(coins.length / itemsPerPage);

    const indexOfLastCoin = currentPage * itemsPerPage;
    const indexOfFirstCoin = indexOfLastCoin - itemsPerPage;
    const currentCoins = coins.slice(indexOfFirstCoin, indexOfLastCoin);

    const handleNextPage = () => {
        if (currentPage < totalPages) {
            setCurrentPage(prevPage => prevPage + 1);
        }
    };

    const handlePrevPage = () => {
        if (currentPage > 1) {
            setCurrentPage(prevPage => prevPage - 1);
        }
    };

    return (
        <div className='table_component'>
            <div className='table_component_header'>
                <p>{tableCaption}</p>
                <DeletePortfolio
                    className='deletePortfolio'
                    setFileUploaded={setFileUploaded}
                    checkIfHasCoins={checkIfHasCoins}
                />
            </div>
            <table>
                <thead>
                    <tr>
                        <th>#</th>
                        <th>Coin</th>
                        <th>Amount</th>
                        <th>Bought For</th>
                        <th>Current Price</th>
                        <th>Change %</th>
                    </tr>
                </thead>
                <tbody>
                    {currentCoins.map((coin, index) =>
                        <CoinRow key={coin.id !== 0 ? coin.id : `${coin.name}-${index}`}
                            coin={coin}
                            index={indexOfFirstCoin + index}
                        />
                    )}
                </tbody>
            </table>
            <div>
                <div className="pagination">
                    <button onClick={handlePrevPage} disabled={currentPage === 1}>Prev</button>
                    <span> Page {currentPage} of {totalPages} </span>
                    <button onClick={handleNextPage} disabled={currentPage === totalPages}>Next</button>
                </div>

            </div>
        </div>
    );
}

export default CoinsTable