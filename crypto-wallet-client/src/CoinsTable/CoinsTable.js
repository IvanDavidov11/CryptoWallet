import React from 'react'
import CoinRow from './CoinRow'

const CoinsTable = ({ coins, tableCaption, hasApiCalculations }) => {
    return (
        <div className='table_component'>
            <table>
                <caption>{tableCaption}</caption>
                <thead>
                    <tr>
                        <th>Coin Name</th>
                        <th>Amount</th>
                        <th>Bought For</th>
                        {hasApiCalculations && (
                            <>
                                <th>Current Price</th>
                                <th>Change from initial price</th>
                            </>
                        )}
                    </tr>
                </thead>
                <tbody>
                    {coins.map((coin, index) =>
                        <CoinRow key={coin.id !== 0 ? coin.id : `${coin.name}-${index}`}
                            coin={coin}
                            hasApiCalculations={hasApiCalculations}
                        />
                    )}
                </tbody>
            </table>
        </div>
    )
}

export default CoinsTable