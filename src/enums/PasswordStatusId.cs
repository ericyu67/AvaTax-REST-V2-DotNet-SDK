using System;

/*
 * AvaTax API Client Library
 *
 * (c) 2004-2017 Avalara, Inc.
 *
 * For the full copyright and license information, please view the LICENSE
 * file that was distributed with this source code.
 *
 * @author Ted Spence
 * @author Zhenya Frolov
 * @author Greg Hester
 */

namespace Avalara.AvaTax.RestClient
{
    /// <summary>
    /// PasswordStatusId
    /// </summary>
    public enum PasswordStatusId
    {
        /// <summary>
        /// UserCannotChange
        /// </summary>
        UserCannotChange,

        /// <summary>
        /// UserCanChange
        /// </summary>
        UserCanChange,

        /// <summary>
        /// UserMustChange
        /// </summary>
        UserMustChange,

    }
}
